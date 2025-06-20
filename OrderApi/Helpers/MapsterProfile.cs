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
                .Map(x => x.count, map => map.Count)
                .Map(x => x.created, map => map.CreatedDate)
                .IgnoreNullValues(true);

            TypeAdapterConfig<CreateOrderDetailsDto, OrderDetails>.NewConfig()
                .Map(x => x.ProductName, map => map.ProductName)
                .Map(x => x.ProductId, map => map.ProductId)
                .Map(x => x.Count, map => map.Count)
                .Map(x => x.Price, map => map.Price)
               .IgnoreNullValues(true);
        }
    }
}
