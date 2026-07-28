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

    public static List<Category> Convert(List<Entity.Category> categories)
        => categories.Adapt<List<Category>>();

    public static Entity.Category Convert(Category category)
        => category.Adapt<Entity.Category>();

    public static Category Convert(Entity.Category category)
        => category.Adapt<Category>();

    public static List<Description> Convert(List<Entity.Description> descriptions)
        => descriptions.Adapt<List<Description>>();

    public static Entity.Description Convert(Description description)
        => description.Adapt<Entity.Description>();

    public static Description Convert(Entity.Description description)
        => description.Adapt<Description>();
}
