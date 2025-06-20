using ProductsApi.DTOs;
using ProductsApi.Helpers;

namespace ProductsApi.Repository;

public interface IProductService
{
    Task<MobileResponse<IEnumerable<GetProductDto>>> GetProductsAsync(CancellationToken ctx);
    Task<MobileResponse<IEnumerable<GetProductDto>>> GetAllAsync(CancellationToken ctx);
    Task<MobileResponse<GetProductDto>> GetByIdAsync(string id, CancellationToken ctx);
    Task<MobileResponse<GetProductDto>> CreateAsync(CreateProductDto model, CancellationToken ctx);
    Task<MobileResponse<GetProductDto>> UpdateAsync(string id, CreateProductDto model, CancellationToken ctx);
    Task<MobileResponse<bool>> DeleteAsync(string id, CancellationToken ctx);
}
