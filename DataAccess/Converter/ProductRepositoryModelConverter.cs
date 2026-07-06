using DatabaseAccess.Repositorymodel;

namespace DatabaseAccess.Converter
{
    public class ProductRepositoryModelConverter
    {
        public static ProductRepositoryModel Convert(IEnumerable<Entity.Product> entities)
        {
            var products = entities.Select(entity => new Repositorymodel.Product(
                            entity.Id.Value,
                            entity.Name,
                            entity.Price,
                            new Repositorymodel.Description(
                                entity.Description.Id.Value,
                                entity.Description.ShortSummary,
                                entity.Description.DetailedText,
                                entity.Description.WeightInGrams
                            ),
                            new Repositorymodel.Category(
                                entity.Category.Id.Value,
                                entity.Category.Name
                            )
                        )).ToList();

            return new ProductRepositoryModel(products);
        }
    }
}
