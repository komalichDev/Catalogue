using Common.Types;
using Shared.Models;

namespace Backend.UseCase.Interactor;

public interface IInteractor
{
    public Task<QueryResult<List<ProductDto>>> GetAllProducts();

    public Task<QueryResult<ProductDto>> GetProductById(ProductId id);
}
