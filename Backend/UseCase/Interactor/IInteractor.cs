using Shared.Models;

namespace Backend.UseCase.Interactor;

public interface IInteractor
{
    public Task<List<ProductDto>> GetAllProducts();
}
