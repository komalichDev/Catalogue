using Common.Types;
using Shared.Models;

namespace Backend.UseCase.Interactor;

public interface IInteractor
{
    public Task<QueryResult<List<ProductDto>>> GetAllProducts();

    public Task<QueryResult<ProductDto>> GetProductById(ProductId id);

    public Task<QueryResult<List<Category>>> GetAllCategories();

    public Task<QueryResult<Category>> GetCategoryById(CategoryId id);

    public Task<QueryResult<Description>> GetDescriptionById(DescriptionId id);

    public Task<QueryResult<List<Description>>> GetAllDescriptions();
}