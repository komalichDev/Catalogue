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

    public Task<Result> CreateProduct(Product product);

    public Task<Result> CreateDescription(Description description);

    public Task<Result> CreateCategory(Category category);

    public Task<Result> UpdateProduct(Product product);

    public Task<Result> UpdateDescription(Description description);

    public Task<Result> UpdateCategory(Category category);

    public Task<Result> DeleteProduct(ProductId id);

    public Task<Result> DeleteCategory(CategoryId id);

    public Task<Result> DeleteDescription(DescriptionId id);
}
