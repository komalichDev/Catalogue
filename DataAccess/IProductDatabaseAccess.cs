using Common.Types;
using DatabaseAccess.Repositorymodel;

namespace DatabaseAccess;

public interface IProductDatabaseAccess
{
    public Task<QueryResult<Repositorymodel.ProductRepositoryModel>> GetAllProducts();
    public Task<QueryResult<Repositorymodel.Product>> GetProduct(ProductId id);

    public Task<Result> CreateProduct(Repositorymodel.Product product);

    public Task<Result> CreateCategory(Repositorymodel.Category category);

    public Task<Result> CreateDescription(Repositorymodel.Description description);

    public Task<Result> DeleteProduct(ProductId id);

    public Task<Result> DeleteCategory(CategoryId id);

    public Task<Result> DeleteDescription(DescriptionId id);

    public Task<Result> UpdateProduct(Repositorymodel.Product product);

    public Task<Result> UpdateCategory(Repositorymodel.Category category);

    public Task<Result> UpdateDescription(Repositorymodel.Description description);
}
