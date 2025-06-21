using Mapster;
using OrderApi.Dtos;
using OrderApi.Models;

namespace OrderApi.Helpers
{
    public sealed class MapsterProfile : TypeAdapterConfig
    {
        public MapsterProfile()
        {
            EmployeeMapping();
        }

        private void EmployeeMapping()
        {
            TypeAdapterConfig<OrderDetails, OrderDetailsDto>.NewConfig()
                .Map(x => x.OrderId, map => map.OrderDetailsId)
                .Map(x => x.productId, map => map.ProductId)
                .Map(x => x.PName, map => map.ProductName)
                .Map(x => x.price, map => map.Price)
                .Map(x => x.stock, map => map.Stock)
                .Map(x => x.created, map => map.CreatedDate)
                .Map(x => x.consumer, map => map.Consumer)
                .Map(x => x.status, map => map.Status)
               .Map(x => x.userId, map => map.UserId)
                .IgnoreNullValues(true);

            TypeAdapterConfig<CreateOrderDetailsDto, OrderDetails>.NewConfig()
                .Map(x => x.ProductName, map => map.ProductName)
                .Map(x => x.ProductId, map => map.ProductId)
                .Map(x => x.Stock, map => map.Stock)
                .Map(x => x.Price, map => map.Price)
                .Map(x => x.Consumer, map => map.Consumer)
                .Map(x => x.Status, map => map.Status)
                .Map(x => x.UserId, map => map.UserId)
               .IgnoreNullValues(true);
        }
    }
}
