using ProductsApi.DTOs;
using ProductsApi.Helpers;

namespace ProductsApi.Repository;

public interface IProductService
{
    Task<MobileResponse<IEnumerable<GetProductDto>>> GetProductsAsync(CancellationToken ctx);
}
