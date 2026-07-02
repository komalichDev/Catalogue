using Backend.UseCase.Interactor.Requestmodel;

namespace Backend.Adapter.Converter;

public class RequestmodelConverter : IRequestmodelConverter
{
    public Requestmodel Convert(Product product)
    {
        var requestmodel = default(Requestmodel);
        requestmodel.Product = new UseCase.Interactor.Requestmodel.Product
        {
            Id = product.Id,
            Name = product.Name,

            Price = product.Price,

            Description = new UseCase.Interactor.Requestmodel.Description
            {
                Id = product.Description.Id,
                ShortText = product.Description.ShortText,
                LongText = product.Description.LongText,
                Weight = product.Description.Weight,
            },

            Category = new UseCase.Interactor.Requestmodel.Category
            {
                Id = product.Category.Id,
                Name = product.Category.Name,
            },
        };

        return requestmodel;
    }
}
