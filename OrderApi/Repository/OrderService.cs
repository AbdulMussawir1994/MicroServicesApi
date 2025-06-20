using Mapster;
using Microsoft.EntityFrameworkCore;
using OrderApi.DbContextClass;
using OrderApi.Dtos;
using OrderApi.Helpers;
using OrderApi.Models;

namespace OrderApi.Repository
{
    public class OrderService : IOrderService
    {
        private readonly DataDbContext _db;
        public OrderService(DataDbContext db) => _db = db;

        public async Task<MobileResponse<IEnumerable<OrderDetailsDto>>> GetAllAsync(CancellationToken ctx)
        {
            var orders = await _db.OrderDetails.AsNoTracking().ToListAsync(ctx);
            return orders.Any()
                ? MobileResponse<IEnumerable<OrderDetailsDto>>.Success(orders.Adapt<IEnumerable<OrderDetailsDto>>(), "Orders Fetched")
                : MobileResponse<IEnumerable<OrderDetailsDto>>.EmptySuccess(Enumerable.Empty<OrderDetailsDto>(), "No Orders Found");
        }

        public async Task<MobileResponse<OrderDetailsDto>> GetByIdAsync(int id, CancellationToken ctx)
        {
            var order = await _db.OrderDetails.AsNoTracking().FirstOrDefaultAsync(o => o.OrderDetailsId == id, ctx);
            return order is null
                ? MobileResponse<OrderDetailsDto>.Fail("Order Not Found")
                : MobileResponse<OrderDetailsDto>.Success(order.Adapt<OrderDetailsDto>(), "Order Fetched");
        }

        public async Task<MobileResponse<OrderDetailsDto>> CreateAsync(CreateOrderDetailsDto model, CancellationToken ctx)
        {
            var order = model.Adapt<OrderDetails>();
            await _db.OrderDetails.AddAsync(order, ctx);
            var result = await _db.SaveChangesAsync(ctx);
            return result > 0
                ? MobileResponse<OrderDetailsDto>.Success(order.Adapt<OrderDetailsDto>(), "Order Created")
                : MobileResponse<OrderDetailsDto>.Fail("Creation Failed");
        }

        public async Task<MobileResponse<OrderDetailsDto>> UpdateAsync(int id, CreateOrderDetailsDto model, CancellationToken ctx)
        {
            var order = await _db.OrderDetails.FirstOrDefaultAsync(o => o.OrderDetailsId == id, ctx);
            if (order == null) return MobileResponse<OrderDetailsDto>.Fail("Order Not Found");

            order.ProductId = model.ProductId;
            order.ProductName = model.ProductName;
            order.Count = model.Count;
            order.Price = model.Price;

            _db.OrderDetails.Update(order);
            var result = await _db.SaveChangesAsync(ctx);
            return result > 0
                ? MobileResponse<OrderDetailsDto>.Success(order.Adapt<OrderDetailsDto>(), "Order Updated")
                : MobileResponse<OrderDetailsDto>.Fail("Update Failed");
        }

        public async Task<MobileResponse<bool>> DeleteAsync(int id, CancellationToken ctx)
        {
            var order = await _db.OrderDetails.FirstOrDefaultAsync(o => o.OrderDetailsId == id, ctx);
            if (order == null) return MobileResponse<bool>.Fail("Order Not Found");

            _db.OrderDetails.Remove(order);
            var result = await _db.SaveChangesAsync(ctx);
            return result > 0
                ? MobileResponse<bool>.EmptySuccess(true, "Order Deleted")
                : MobileResponse<bool>.Fail("Delete Failed");
        }
    }
}
