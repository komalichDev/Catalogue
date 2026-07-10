using Backend.Entity;
using Backend.Repository.Converter;
using Backend.UseCase.Interactor;
using Common.Types;
using DatabaseAccess;

namespace Backend.Repository;

public class ProductRepository : IProductGateway
{
    private IProductDatabaseAccess _databaseAccess;

    public ProductRepository(IProductDatabaseAccess databaseAccess)
        => _databaseAccess = databaseAccess;

    public async Task<QueryResult<List<Category>>> GetAllCategories()
            => await ExecuteQueryAsync(
                () => _databaseAccess.GetAllCategories(),
                data => ProductConverter.Convert(data),
                new List<DatabaseAccess.RepositoryModel.Category>());

    public async Task<QueryResult<List<Description>>> GetAllDescriptions()
        => await ExecuteQueryAsync(
            () => _databaseAccess.GetAllDescriptions(),
            data => ProductConverter.Convert(data),
            new List<DatabaseAccess.RepositoryModel.Description>());

    public async Task<QueryResult<List<Product>>> GetAllProducts()
        => await ExecuteQueryAsync(
            () => _databaseAccess.GetAllProducts(),
            data => ProductConverter.Convert(data),
            new DatabaseAccess.RepositoryModel.ProductRepositoryModel(new List<DatabaseAccess.RepositoryModel.Product>()));

    public async Task<QueryResult<Category>> GetCategoryById(CategoryId id)
        => await ExecuteQueryAsync(
            () => _databaseAccess.GetCategory(id),
            data => ProductConverter.Convert(data));

    public async Task<QueryResult<Description>> GetDescriptionById(DescriptionId id)
        => await ExecuteQueryAsync(
            () => _databaseAccess.GetDescription(id),
            data => ProductConverter.Convert(data));

    public async Task<QueryResult<Product>> GetProductById(ProductId id)
        => await ExecuteQueryAsync(
            () => _databaseAccess.GetProduct(id),
            data => ProductConverter.Convert(data));

    public async Task<Result> CreateProduct(Product product)
        => await ExecuteOperationAsync(() => _databaseAccess.CreateProduct(ProductConverter.Convert(product)));

    public async Task<Result> CreateDescription(Description description)
    => await ExecuteOperationAsync(() => _databaseAccess.CreateDescription(ProductConverter.Convert(description)));

    public async Task<Result> CreateCategory(Category category)
        => await ExecuteOperationAsync(() => _databaseAccess.CreateCategory(ProductConverter.Convert(category)));

    public async Task<Result> UpdateProduct(Product product)
    => await ExecuteOperationAsync(() => _databaseAccess.UpdateProduct(ProductConverter.Convert(product)));

    public async Task<Result> UpdateDescription(Description description)
    => await ExecuteOperationAsync(() => _databaseAccess.UpdateDescription(ProductConverter.Convert(description)));

    public async Task<Result> UpdateCategory(Category category)
        => await ExecuteOperationAsync(() => _databaseAccess.UpdateCategory(ProductConverter.Convert(category)));

    public async Task<Result> DeleteProduct(ProductId id)
        => await ExecuteOperationAsync(() => _databaseAccess.DeleteProduct(id));

    public async Task<Result> DeleteCategory(CategoryId id)
        => await ExecuteOperationAsync(() => _databaseAccess.DeleteCategory(id));

    public async Task<Result> DeleteDescription(DescriptionId id)
        => await ExecuteOperationAsync(() => _databaseAccess.DeleteDescription(id));

    private static async Task<QueryResult<TResult>> ExecuteQueryAsync<TData, TResult>(
        Func<Task<QueryResult<TData>>> dbCall,
        Func<TData, TResult> converter,
        TData? fallbackData = null)
        where TResult : class
        where TData : class
    {
        var dbResult = await dbCall();

        if (!dbResult.IsSuccess)
        {
            return (QueryResult<TResult>)Result.Failure(dbResult.ErrorCode);
        }

        var data = dbResult.Data ?? fallbackData;
        if (data == null)
        {
            return (QueryResult<TResult>)Result.Failure(dbResult.ErrorCode);
        }

        return QueryResult<TResult>.Success(converter(data));
    }

    private static async Task<Result> ExecuteOperationAsync(Func<Task<Result>> operation)
    {
        var dbResult = await operation();
        if (!dbResult.IsSuccess)
        {
            return Result.Failure(dbResult.ErrorCode);
        }

        return Result.Success();
    }
}
