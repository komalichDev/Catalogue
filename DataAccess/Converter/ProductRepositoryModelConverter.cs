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
    }
}
