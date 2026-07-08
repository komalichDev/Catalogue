using Backend.Entity;
using Common.Types;

namespace Backend.UseCase.Interactor;

public interface IProductGateway
{
    public Task<QueryResult<List<Product>>> GetAllProducts();

    public Task<QueryResult<Product>> GetProductById(ProductId id);

    public Task<QueryResult<List<Category>>> GetAllCategories();

    public Task<QueryResult<Category>> GetCategoryById(CategoryId id);

    public Task<QueryResult<Description>> GetDescriptionById(DescriptionId id);

    public Task<QueryResult<List<Description>>> GetAllDescriptions();
}
