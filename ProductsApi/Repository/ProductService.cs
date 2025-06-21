using Mapster;
using Microsoft.EntityFrameworkCore;
using ProductsApi.DataContextClass;
using ProductsApi.DTOs;
using ProductsApi.Helpers;
using ProductsApi.Models;
using ProductsApi.RabbitMqProducer;
using ProductsApi.Utilities;

namespace ProductsApi.Repository
{
    public class ProductService : IProductService
    {
        private readonly DataContext _db;
        private readonly IContextUser _contextUser;
        private readonly IRabbitMqService _rabbitMqService;

        public ProductService(DataContext db, IContextUser contextUser, IRabbitMqService rabbitMqService)
        {
            _db = db;
            _contextUser = contextUser;
            _rabbitMqService = rabbitMqService;
        }

        public async Task<MobileResponse<IEnumerable<GetProductDto>>> GetAllAsync(CancellationToken ctx)
        {
            try
            {
                var products = await _db.Products.AsNoTracking().ToListAsync(ctx);

                if (!products.Any())
                    return MobileResponse<IEnumerable<GetProductDto>>.EmptySuccess(Enumerable.Empty<GetProductDto>(), "No Products Found");

                var mapped = products.Adapt<IEnumerable<GetProductDto>>();
                return MobileResponse<IEnumerable<GetProductDto>>.Success(mapped, "Products Fetched Successfully");
            }
            catch (Exception ex)
            {
                return MobileResponse<IEnumerable<GetProductDto>>.Fail($"An error occurred: {ex.Message}");
            }
        }

        public async Task<MobileResponse<GetProductDto>> GetByIdAsync(string id, CancellationToken ctx)
        {
            try
            {
                var product = await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id, ctx);

                return product is null
                    ? MobileResponse<GetProductDto>.Fail("Product Not Found")
                    : MobileResponse<GetProductDto>.Success(product.Adapt<GetProductDto>(), "Product Fetched Successfully");
            }
            catch (Exception ex)
            {
                return MobileResponse<GetProductDto>.Fail($"An error occurred: {ex.Message}");
            }
        }

        public async Task<MobileResponse<GetProductDto>> CreateAsync(CreateProductDto model, CancellationToken ctx)
        {
            try
            {
                var product = model.Adapt<Product>();
                product.CreatedBy = _contextUser.UserId;

                await _db.Products.AddAsync(product, ctx);
                var result = await _db.SaveChangesAsync(ctx);

                var rabbitMq = new ProductMessageDto
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    ProductPrice = product.ProductPrice,
                    ProductCategory = product.ProductCategory,
                    Quantity = product.Quantity,
                    Consumer = _contextUser?.Email ?? "Not Found",
                    User = _contextUser?.UserId ?? "1"
                };

                _rabbitMqService.PublishMessage("ProductQueue", rabbitMq);

                return result > 0
                    ? MobileResponse<GetProductDto>.Success(product.Adapt<GetProductDto>(), "Product Created and message sent to RabbitMQ.")
                    : MobileResponse<GetProductDto>.Fail("Failed to Create Product");
            }
            catch (Exception ex)
            {
                return MobileResponse<GetProductDto>.Fail($"An error occurred: {ex.Message}");
            }
        }

        public async Task<MobileResponse<GetProductDto>> UpdateAsync(string id, CreateProductDto model, CancellationToken ctx)
        {
            try
            {
                var product = await _db.Products.FirstOrDefaultAsync(p => p.ProductId == id, ctx);

                if (product is null)
                    return MobileResponse<GetProductDto>.Fail("Product Not Found");

                model.Adapt(product); // Efficiently map updated fields to entity

                _db.Products.Update(product);
                var result = await _db.SaveChangesAsync(ctx);

                return result > 0
                    ? MobileResponse<GetProductDto>.Success(product.Adapt<GetProductDto>(), "Product Updated Successfully")
                    : MobileResponse<GetProductDto>.Fail("Failed to Update Product");
            }
            catch (Exception ex)
            {
                return MobileResponse<GetProductDto>.Fail($"An error occurred: {ex.Message}");
            }
        }

        public async Task<MobileResponse<bool>> DeleteAsync(string id, CancellationToken ctx)
        {
            try
            {
                var product = await _db.Products.FirstOrDefaultAsync(p => p.ProductId == id, ctx);

                if (product is null)
                    return MobileResponse<bool>.Fail("Product Not Found");

                _db.Products.Remove(product);
                var result = await _db.SaveChangesAsync(ctx);

                return result > 0
                    ? MobileResponse<bool>.EmptySuccess(true, "Product Deleted Successfully")
                    : MobileResponse<bool>.Fail("Failed to Delete Product");
            }
            catch (Exception ex)
            {
                return MobileResponse<bool>.Fail($"An error occurred: {ex.Message}");
            }
        }

        public async Task<MobileResponse<IEnumerable<GetProductDto>>> GetProductsAsync(CancellationToken ctx)
        {
            try
            {
                var response = await _db.Products.AsNoTracking().ToListAsync(ctx);

                var products = response.Adapt<IEnumerable<GetProductDto>>();

                return response.Any()
                    ? MobileResponse<IEnumerable<GetProductDto>>.Success(products, "Products Fetched Successfully")
                    : MobileResponse<IEnumerable<GetProductDto>>.EmptySuccess(Enumerable.Empty<GetProductDto>(), "No Employees Found.");
            }
            catch (Exception ex)
            {
                return MobileResponse<IEnumerable<GetProductDto>>.Fail($"An error Occured: {ex.Message}", "400");
            }
        }
    }
}
