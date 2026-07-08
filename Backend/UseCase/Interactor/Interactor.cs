using Backend.UseCase.Interactor.Converter;
using Common.Types;
using Shared.Models;

namespace Backend.UseCase.Interactor;

public class Interactor : IInteractor
{
    private IProductGateway _gateway;

    public Interactor(IProductGateway gateway)
        => _gateway = gateway;

    public async Task<QueryResult<List<ProductDto>>> GetAllProducts()
            => await ExecuteQueryAsync(
                () => _gateway.GetAllProducts(),
                data => ProductDtoConverter.Convert(data),
                new List<Entity.Product>());

    public async Task<QueryResult<ProductDto>> GetProductById(ProductId id)
        => await ExecuteQueryAsync(
            () => _gateway.GetProductById(id),
            data => ProductDtoConverter.Convert(data));

    public async Task<QueryResult<Description>> GetDescriptionById(DescriptionId id)
        => await ExecuteQueryAsync(
            () => _gateway.GetDescriptionById(id),
            data => ProductDtoConverter.Convert(data));

    public async Task<QueryResult<Category>> GetCategoryById(CategoryId id)
        => await ExecuteQueryAsync(
            () => _gateway.GetCategoryById(id),
            data => ProductDtoConverter.Convert(data));

    public async Task<QueryResult<List<Category>>> GetAllCategories()
        => await ExecuteQueryAsync(
            () => _gateway.GetAllCategories(),
            data => ProductDtoConverter.Convert(data));

    public async Task<QueryResult<List<Description>>> GetAllDescriptions()
        => await ExecuteQueryAsync(
            () => _gateway.GetAllDescriptions(),
            data => ProductDtoConverter.Convert(data));

    private async Task<QueryResult<TResult>> ExecuteQueryAsync<TData, TResult>(
        Func<Task<QueryResult<TData>>> gatewayCall,
        Func<TData, TResult> converter,
        TData? fallbackData = null)
        where TResult : class
        where TData : class
    {
        var result = await gatewayCall();

        if (!result.IsSuccess)
        {
            return (QueryResult<TResult>)QueryResult<TResult>.Failure(result.ErrorCode);
        }

        var data = result.Data ?? fallbackData;
        if (data == null)
        {
            return (QueryResult<TResult>)QueryResult<TResult>.Failure(result.ErrorCode);
        }

        return QueryResult<TResult>.Success(converter(data));
    }
}
