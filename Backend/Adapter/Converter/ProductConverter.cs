using Backend.UseCase.Interactor.Responsemodel;

namespace Backend.Adapter.Converter;

public class ProductConverter : IProductConverter
{
    public List<Product> Convert(Responsemodel model)
    {
        var response = new List<Product>();

        if (model.Products == null)
        {
            return response;
        }

        foreach (var repoProduct in model.Products)
        {
            var respProduct = new UseCase.Interactor.Responsemodel.Product
            {
                Id = repoProduct.Id,
                Name = repoProduct.Name,

                Price = (int)repoProduct.Price,

                IsSucess = true,

                Description = new UseCase.Interactor.Responsemodel.Description
                {
                    Id = repoProduct.Description.Id,
                    ShortText = repoProduct.Description.ShortText,
                    LongText = repoProduct.Description.LongText,
                    Weight = repoProduct.Description.Weight,
                },

                Category = new UseCase.Interactor.Responsemodel.Category
                {
                    Id = repoProduct.Category.Id,
                    Name = repoProduct.Category.Name,
                },
            };

            response.Add(respProduct);
        }

        return response;
    }
}
