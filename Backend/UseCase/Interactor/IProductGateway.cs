using Backend.Entity;

namespace Backend.UseCase.Interactor;

public interface IProductGateway
{
    public List<Product> GetAllProducts();
}
