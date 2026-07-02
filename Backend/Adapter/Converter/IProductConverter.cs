using Backend.UseCase.Interactor.Responsemodel;

namespace Backend.Adapter.Converter;

public interface IProductConverter
{
    public List<Product> Convert(Responsemodel model);
}
