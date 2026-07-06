using DatabaseAccess.Repositorymodel;

namespace DatabaseAccess.Converter
{
    public class ProductRepositoryModelConverter
    {
        public static ProductRepositoryModel Convert(IEnumerable<Entity.Product> entities)
        {
            var resultModel = new ProductRepositoryModel();
            resultModel.Products = new List<Repositorymodel.Product>();

            foreach (var entity in entities) {
                var product = new Repositorymodel.Product(
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
                );

                resultModel.Products.Add(product);
            }

            return resultModel;
        }
    }
}
