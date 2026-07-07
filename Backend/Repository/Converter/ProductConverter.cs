using DatabaseAccess.Repositorymodel;
using Mapster;

namespace Backend.Repository.Converter;

public class ProductConverter
{
    public static List<Entity.Product> Convert(ProductRepositoryModel model)
    {
        if (model?.Products == null)
        {
            return new List<Entity.Product>();
        }

        return model.Products.Adapt<List<Entity.Product>>();
    }

    public static ProductRepositoryModel Convert(List<Entity.Product> products)
        => new ProductRepositoryModel(products.Adapt<List<DatabaseAccess.Repositorymodel.Product>>());

    public static DatabaseAccess.Repositorymodel.Product Convert(Entity.Product product)
        => product.Adapt<DatabaseAccess.Repositorymodel.Product>();
}
