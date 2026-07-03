using Backend.UseCase.Interactor.Converter;
using Shared.Models;

namespace Backend.UseCase.Interactor;

public class Interactor : IInteractor
{
    private IProductGateway _gateway;

    public Interactor(IProductGateway gateway)
    {
        _gateway = gateway;
    }

    public List<ProductDto> GetAllProducts()
    {
        var result = _gateway.GetAllProducts();
        return ProductDtoConverter.Convert(result);
    }
}
