using Mapster;
using ProductsApi.DTOs;
using ProductsApi.Models;

namespace ProductsApi.Helpers;

public sealed class MapsterProfile : TypeAdapterConfig
{
    public MapsterProfile()
    {
        EmployeeMapping();
    }

    private void EmployeeMapping()
    {
        TypeAdapterConfig<Product, GetProductDto>.NewConfig()
            .Map(x => x.id, map => map.ProductId)
            .Map(x => x.product, map => map.ProductName)
            .Map(x => x.description, map => map.ProductDescription)
            .Map(x => x.category, map => map.ProductCategory)
            .Map(x => x.price, map => map.ProductPrice)
            .Map(x => x.created, map => map.CreatedDate)
            .Map(x => x.createdBy, map => map.CreatedBy)
            .IgnoreNullValues(true);

        //TypeAdapterConfig<CreateEmployeeDto, Employee>.NewConfig()
        //    .Map(x => x.Name, map => map.Name)
        //    .Map(x => x.Email, map => map.Email)
        //    .Map(x => x.Salary, map => map.Salary)
        //    .Map(x => x.DepartmentId, map => map.DepartmentId)
        //   .IgnoreNullValues(true);
    }
}
