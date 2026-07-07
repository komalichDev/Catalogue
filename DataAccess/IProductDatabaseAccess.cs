using Common.Types;
using DatabaseAccess.RepositoryModel;

namespace DatabaseAccess;

public interface IProductDatabaseAccess
{
    public Task<QueryResult<RepositoryModel.ProductRepositoryModel>> GetAllProducts();
    public Task<QueryResult<RepositoryModel.Product>> GetProduct(ProductId id);

    public Task<Result> CreateProduct(RepositoryModel.Product product);

    public Task<Result> CreateCategory(RepositoryModel.Category category);

    public Task<Result> CreateDescription(RepositoryModel.Description description);

    public Task<Result> DeleteProduct(ProductId id);

    public Task<Result> DeleteCategory(CategoryId id);

    public Task<Result> DeleteDescription(DescriptionId id);

    public Task<Result> UpdateProduct(RepositoryModel.Product product);

    public Task<Result> UpdateCategory(RepositoryModel.Category category);

    public Task<Result> UpdateDescription(RepositoryModel.Description description);
}
