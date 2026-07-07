using DatabaseAccess.RepositoryModel;
using Mapster;

namespace DatabaseAccess.Converter
{
    public class ProductRepositoryModelConverter
    {
        public static ProductRepositoryModel Convert(IEnumerable<Entity.Product> entities)
            => new ProductRepositoryModel(entities.Adapt<List<RepositoryModel.Product>>());

        public static IEnumerable<Entity.Product> Convert(ProductRepositoryModel repositoryModel)
        {
            if (repositoryModel?.Products == null) {
                return new List<Entity.Product>();
            }

            return repositoryModel.Products.Adapt<IEnumerable<Entity.Product>>();
        }

        public static Entity.Product Convert(RepositoryModel.Product product)
            => product.Adapt<Entity.Product>();

        public static Entity.Category Convert(RepositoryModel.Category category)
            => category.Adapt<Entity.Category>();

        public static Entity.Description Convert(RepositoryModel.Description description)
            => description.Adapt<Entity.Description>();

        public static RepositoryModel.Product Convert(Entity.Product entity)
            => entity.Adapt<RepositoryModel.Product>();
    }
}
