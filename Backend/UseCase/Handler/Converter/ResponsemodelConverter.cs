using Repo = DataAccess.Repositorymodel;
using Resp = Backend.UseCase.Interactor.Responsemodel;

namespace Backend.UseCase.Handler.Converter;

public class ResponsemodelConverter : IResponsemodelConverter
{
    public Resp.Responsemodel ConvertToResponsemodel(Repo.Repositorymodel model)
    {
        var response = new Resp.Responsemodel
        {
            Products = new List<Resp.Product>(),
        };

        if (model.Products == null)
        {
            return response;
        }

        foreach (var repoProduct in model.Products)
        {
            var respProduct = new Resp.Product
            {
                Id = repoProduct.Id,
                Name = repoProduct.Name,

                Price = (int)repoProduct.Price,

                IsSucess = true,

                Description = new Resp.Description
                {
                    Id = repoProduct.Description.Id,
                    ShortText = repoProduct.Description.ShortText,
                    LongText = repoProduct.Description.LongText,
                    Weight = repoProduct.Description.Weight,
                },

                Category = new Resp.Category
                {
                    Id = repoProduct.Category.Id,
                    Name = repoProduct.Category.Name,
                },
            };

            response.Products.Add(respProduct);
        }

        return response;
    }
}