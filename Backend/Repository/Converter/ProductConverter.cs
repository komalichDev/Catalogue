using DatabaseAccess.Repositorymodel;

namespace Backend.Repository.Converter;

public class ProductConverter
{
    public static List<Entity.Product> Convert(ProductRepositoryModel model)
    {
        List<Entity.Product> products = new List<Entity.Product>();
        foreach (var product in model.Products)
        {
            var productEntity = new Entity.Product(
                product.Id,
                product.Name,
                product.Price,
                new Entity.Description(
                    product.Description.Id,
                    product.Description.ShortText,
                    product.Description.LongText,
                    product.Description.Weight),
                new Entity.Category(
                    product.Category.Id,
                    product.Category.Name));
            products.Add(productEntity);
        }

        return products;
    }

    public static ProductRepositoryModel Convert(List<Entity.Product> products)
    {
        var repoProducts = new List<DatabaseAccess.Repositorymodel.Product>();

        foreach (var product in products)
        {
            repoProducts.Add(Convert(product));
        }

        return new ProductRepositoryModel
        {
            Products = repoProducts,
        };
    }

    public static DatabaseAccess.Repositorymodel.Product Convert(Entity.Product product)
    {
        var productModel = new DatabaseAccess.Repositorymodel.Product(
            product.Id,
            product.Name,
            product.Price,
            new DatabaseAccess.Repositorymodel.Description(
                product.Description.Id,
                product.Description.ShortText,
                product.Description.LongText,
                product.Description.Weight),
            new DatabaseAccess.Repositorymodel.Category(
                product.Category.Id,
                product.Category.Name));

        return productModel;
    }
}
