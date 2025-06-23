using KmacHelper.ConsumerModel;
using Mapster;
using Microsoft.EntityFrameworkCore;
using OrderApi.DbContextClass;
using OrderApi.Dtos;
using OrderApi.Helpers;
using OrderApi.Models;
using OrderApi.RabbitMqProducer;
using OrderApi.Utilities.ContextHelper;
using System.Text.Json;

namespace OrderApi.Repository
{
    public class OrderService : IOrderService
    {
        private readonly DataDbContext _db;
        private readonly IContextUser _contextUser;
        private readonly IRabbitMqService _rabbitMqService;
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderService(DataDbContext db, IContextUser contextUser, IRabbitMqService rabbitMqService, IHttpClientFactory httpClientFactory)
        {
            _db = db;
            _contextUser = contextUser;
            _rabbitMqService = rabbitMqService;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<MobileResponse<IEnumerable<OrderDetailsDto>>> GetAllAsync(CancellationToken ctx)
        {
            var orders = await _db.OrderDetails.AsNoTracking().ToListAsync(ctx);
            return orders.Any()
                ? MobileResponse<IEnumerable<OrderDetailsDto>>.Success(orders.Adapt<IEnumerable<OrderDetailsDto>>(), "Orders Fetched")
                : MobileResponse<IEnumerable<OrderDetailsDto>>.EmptySuccess(Enumerable.Empty<OrderDetailsDto>(), "No Orders Found");
        }

        public async Task<MobileResponse<OrderDetailsDto>> GetByIdAsync(int id, CancellationToken ctx)
        {
            var order = await _db.OrderDetails.AsNoTracking().FirstOrDefaultAsync(o => o.OrderId == id, ctx);
            return order is null
                ? MobileResponse<OrderDetailsDto>.Fail("Order Not Found")
                : MobileResponse<OrderDetailsDto>.Success(order.Adapt<OrderDetailsDto>(), "Order Fetched");
        }

        public async Task<MobileResponse<OrderDetailsDto>> CreateAsync(CreateOrderDetailsDto model, CancellationToken ctx)
        {
            var order = model.Adapt<OrderDetails>();

            var client = _httpClientFactory.CreateClient("ProductApi");

            // Assuming the base URL is already set, only append relative path
            var productResponse = await client.GetAsync($"products/{model.ProductId}", ctx);

            if (!productResponse.IsSuccessStatusCode)
            {
                return MobileResponse<OrderDetailsDto>.Fail("Failed to fetch product details from external service.");
            }

            var productContent = await productResponse.Content.ReadAsStringAsync(ctx);
            var productData = JsonSerializer.Deserialize<ProductDto>(productContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (productData == null)
            {
                return MobileResponse<OrderDetailsDto>.Fail("Product data was empty.");
            }

            // ✅ Populate order with external data
            order.UserId = _contextUser?.UserId ?? "1";
            order.Status = "Pending";
            order.ProductName = productData.ProductName;
            order.Consumer = _contextUser?.Email ?? "Not Found";

            var rabbitMq = new OrderMessageDto
            {
                OrderId = order.OrderId,
                ProductId = order.ProductId,
                UserId = order.UserId,
                ProductName = order.ProductName,
                Consumer = order.Consumer,
                CreatedDate = order.CreatedDate,
                Status = order.Status,
                TotalOrders = order.TotalOrders,
            };

            _rabbitMqService.PublishMessage("OrderQueue", rabbitMq);

            await _db.OrderDetails.AddAsync(order, ctx);
            var result = await _db.SaveChangesAsync(ctx);
            return result > 0
                ? MobileResponse<OrderDetailsDto>.Success(order.Adapt<OrderDetailsDto>(), "Order Created")
                : MobileResponse<OrderDetailsDto>.Fail("Creation Failed");
        }

        //public async Task<MobileResponse<bool>> AutoCreateAsync(ProductDto model)
        //{
        //    var order = new OrderDetails
        //    {
        //        ProductId = model.ProductId,
        //        ProductName = model.ProductName,
        //        //   Stock = model.Quantity,
        //        Consumer = model.Consumer ?? "Unknown Customer",
        //        Status = "Confirmed",
        //        CreatedDate = DateTime.UtcNow,
        //        //   Price = model.ProductPrice,
        //        UserId = model.User,
        //    };

        //    await _db.OrderDetails.AddAsync(order);
        //    var result = await _db.SaveChangesAsync();

        //    return result > 0
        //        ? MobileResponse<bool>.Success(true, "Order Created")
        //        : MobileResponse<bool>.Fail("Creation Failed");
        //}

        public async Task<MobileResponse<OrderDetailsDto>> UpdateAsync(int id, CreateOrderDetailsDto model, CancellationToken ctx)
        {
            var order = await _db.OrderDetails.FirstOrDefaultAsync(o => o.OrderId == id, ctx);
            if (order == null) return MobileResponse<OrderDetailsDto>.Fail("Order Not Found");

            order.ProductId = model.ProductId;
            // order.ProductName = model.ProductName;
            //    order.Stock = model.Stock;
            //   order.Price = model.Price;

            _db.OrderDetails.Update(order);
            var result = await _db.SaveChangesAsync(ctx);
            return result > 0
                ? MobileResponse<OrderDetailsDto>.Success(order.Adapt<OrderDetailsDto>(), "Order Updated")
                : MobileResponse<OrderDetailsDto>.Fail("Update Failed");
        }

        public async Task<MobileResponse<bool>> DeleteAsync(int id, CancellationToken ctx)
        {
            var order = await _db.OrderDetails.FirstOrDefaultAsync(o => o.OrderId == id, ctx);
            if (order == null) return MobileResponse<bool>.Fail("Order Not Found");

            _db.OrderDetails.Remove(order);
            var result = await _db.SaveChangesAsync(ctx);
            return result > 0
                ? MobileResponse<bool>.EmptySuccess(true, "Order Deleted")
                : MobileResponse<bool>.Fail("Delete Failed");
        }
    }
}
