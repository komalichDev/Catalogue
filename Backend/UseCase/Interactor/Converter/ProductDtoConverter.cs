using Mapster;
using Shared.Models;

namespace Backend.UseCase.Interactor.Converter;

public class ProductDtoConverter
{
    public static ProductDto Convert(Entity.Product product)
        => product.Adapt<ProductDto>();

    public static Entity.Product Convert(ProductDto productDto)
        => productDto.Adapt<Entity.Product>();

    public static List<ProductDto> Convert(List<Entity.Product> products)
        => products.Adapt<List<ProductDto>>();
}
