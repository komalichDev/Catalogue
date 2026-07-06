using Backend.Entity;

namespace Backend.UseCase.Interactor;

public interface IProductGateway
{
    public Task<List<Product>> GetAllProducts();
}
