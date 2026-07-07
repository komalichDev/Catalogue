using Backend.Entity;
using Common.Types;

namespace Backend.UseCase.Interactor;

public interface IProductGateway
{
    public Task<QueryResult<List<Product>>> GetAllProducts();

    public Task<QueryResult<Product>> GetOneProduct(ProductId id);
}
