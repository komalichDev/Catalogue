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
}
