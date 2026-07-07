using DatabaseAccess.RepositoryModel;
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

    public static Entity.Product Convert(DatabaseAccess.RepositoryModel.Product product)
        => product.Adapt<Entity.Product>();

    public static ProductRepositoryModel Convert(List<Entity.Product> products)
        => new ProductRepositoryModel(products.Adapt<List<DatabaseAccess.RepositoryModel.Product>>());

    public static DatabaseAccess.RepositoryModel.Product Convert(Entity.Product product)
        => product.Adapt<DatabaseAccess.RepositoryModel.Product>();
}
