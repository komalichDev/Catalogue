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

    public Task<Result> CreateProduct(ProductDto product);

    public Task<Result> CreateCategory(Category category);

    public Task<Result> UpdateProduct(ProductDto product);

    public Task<Result> UpdateCategory(Category category);

    public Task<Result> DeleteProduct(ProductDto product);

    public Task<Result> DeleteCategory(CategoryId category);
}