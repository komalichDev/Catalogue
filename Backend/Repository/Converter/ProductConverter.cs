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

    public static DatabaseAccess.RepositoryModel.Product Convert(Entity.Product product)
        => product.Adapt<DatabaseAccess.RepositoryModel.Product>();

    public static ProductRepositoryModel Convert(List<Entity.Product> products)
        => new ProductRepositoryModel(products.Adapt<List<DatabaseAccess.RepositoryModel.Product>>());

    public static Category Convert(Entity.Category category)
        => category.Adapt<Category>();

    public static List<Entity.Category> Convert(List<Category> categories)
        => categories.Adapt<List<Entity.Category>>();

    public static Entity.Category Convert(Category category)
        => category.Adapt<Entity.Category>();

    public static List<Description> Convert(List<Entity.Description> descriptions)
        => descriptions.Adapt<List<Description>>();

    public static Entity.Description Convert(Description description)
        => description.Adapt<Entity.Description>();

    public static Description Convert(Entity.Description description)
        => description.Adapt<Description>();

    public static List<Entity.Description> Convert(List<Description> descriptions)
        => descriptions.Adapt<List<Entity.Description>>();
}
