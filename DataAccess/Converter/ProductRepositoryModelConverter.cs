using DatabaseAccess.Repositorymodel;
using Mapster;

namespace DatabaseAccess.Converter
{
    public class ProductRepositoryModelConverter
    {
        public static ProductRepositoryModel Convert(IEnumerable<Entity.Product> entities)
            => new ProductRepositoryModel(entities.Adapt<List<Repositorymodel.Product>>());

        public static IEnumerable<Entity.Product> Convert(ProductRepositoryModel repositoryModel)
        {
            if (repositoryModel?.Products == null) {
                return new List<Entity.Product>();
            }

            return repositoryModel.Products.Adapt<IEnumerable<Entity.Product>>();
        }

        public static Entity.Product Convert(Repositorymodel.Product product)
            => product.Adapt<Entity.Product>();

        public static Entity.Category Convert(Repositorymodel.Category category)
            => category.Adapt<Entity.Category>();

        public static Entity.Description Convert(Repositorymodel.Description description)
            => description.Adapt<Entity.Description>();

        public static Repositorymodel.Product Convert(Entity.Product entity)
            => entity.Adapt<Repositorymodel.Product>();
    }
}
