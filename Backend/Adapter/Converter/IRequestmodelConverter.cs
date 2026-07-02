using Backend.UseCase.Interactor.Requestmodel;

namespace Backend.Adapter.Converter;

public interface IRequestmodelConverter
{
    public Requestmodel Convert(Product product);
}
