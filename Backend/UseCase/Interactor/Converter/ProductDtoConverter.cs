using Shared.Models;

namespace Backend.UseCase.Interactor.Converter;

public class ProductDtoConverter
{
    public static ProductDto Convert(Entity.Product product)
    {
        var productDto = new ProductDto(
            product.Id,
            product.Name,
            product.Price,
            new Shared.Models.Description(
                product.Description.Id,
                product.Description.ShortText,
                product.Description.LongText,
                product.Description.Weight),
            new Shared.Models.Category(
                product.Category.Id,
                product.Category.Name));

        return productDto;
    }

    public static Entity.Product Convert(ProductDto productDto)
    {
        var product = new Entity.Product(
            productDto.Id,
            productDto.Name,
            productDto.Price,
            new Entity.Description(
                productDto.Description.Id,
                productDto.Description.ShortDescription,
                productDto.Description.LongDescription,
                productDto.Description.Weight),
            new Entity.Category(
                productDto.Category.Id,
                productDto.Category.Name));

        return product;
    }

    public static List<ProductDto> Convert(List<Entity.Product> products)
    {
        var productsDto = new List<ProductDto>();

        foreach (var product in products)
        {
            productsDto.Add(Convert(product));
        }

        return productsDto;
    }
}
