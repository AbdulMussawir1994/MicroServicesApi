using Mapster;
using Microsoft.EntityFrameworkCore;
using ProductsApi.DataContextClass;
using ProductsApi.DTOs;
using ProductsApi.Helpers;

namespace ProductsApi.Repository
{
    public class ProductService : IProductService
    {
        private readonly DataContext _db;
        public ProductService(DataContext db)
        {
            _db = db;
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
                return MobileResponse<IEnumerable<GetProductDto>>.Fail($"An error Occured: {ex.Message}", false);
            }
        }
    }
}
