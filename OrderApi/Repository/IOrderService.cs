using OrderApi.Dtos;
using OrderApi.Helpers;

namespace OrderApi.Repository
{
    public interface IOrderService
    {
        Task<MobileResponse<IEnumerable<OrderDetailsDto>>> GetAllAsync(CancellationToken ctx);
        Task<MobileResponse<OrderDetailsDto>> GetByIdAsync(int id, CancellationToken ctx);
        Task<MobileResponse<OrderDetailsDto>> CreateAsync(CreateOrderDetailsDto model, CancellationToken ctx);
        //Task<MobileResponse<bool>> AutoCreateAsync(Product model);
        Task<MobileResponse<OrderDetailsDto>> UpdateAsync(int id, CreateOrderDetailsDto model, CancellationToken ctx);
        Task<MobileResponse<bool>> DeleteAsync(int id, CancellationToken ctx);
    }
}
