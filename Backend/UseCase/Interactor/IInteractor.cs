using Shared.Models;

namespace Backend.UseCase.Interactor;

public interface IInteractor
{
    public List<ProductDto> GetAllProducts();
}
