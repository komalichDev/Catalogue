using Backend.UseCase.Interactor;
using Common.Exception;
using Common.Types;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Backend.Adapter;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private IInteractor _interactor;

    public ProductController(IInteractor interactor) 
        => _interactor = interactor;

    [HttpGet]
    public async Task<QueryResult<List<ProductDto>>> GetProducts()
         => await ExecuteQueryAsync(
             () => _interactor.GetAllProducts(),
             data => data,
             new List<ProductDto>());

    [HttpGet("Product/{productId}")]
    public async Task<QueryResult<List<ProductDto>>> GetProduct([FromRoute] ProductId productId)
        => await ExecuteQueryAsync(
            () => _interactor.GetProductById(productId),
            data => new List<ProductDto> { data });

    [HttpGet("Category/")]
    public async Task<QueryResult<List<Category>>> GetCategories()
        => await ExecuteQueryAsync(
            () => _interactor.GetAllCategories(),
            data => data);

    [HttpGet("Category/{categoryId}")]
    public async Task<QueryResult<List<Category>>> GetCategory([FromRoute] CategoryId categoryId)
        => await ExecuteQueryAsync(
            () => _interactor.GetCategoryById(categoryId),
            data => new List<Category> { data });

    [HttpGet("Description/{descriptionId}")]
    public async Task<QueryResult<Description>> GetDescription([FromRoute] DescriptionId descriptionId)
        => await ExecuteQueryAsync(
            () => _interactor.GetDescriptionById(descriptionId),
            data => data);

    [HttpGet("Description/")]
    public async Task<QueryResult<List<Description>>> GetDescriptions()
        => await ExecuteQueryAsync(
            () => _interactor.GetAllDescriptions(),
            data => data);

    [HttpPost]
    public async Task<Result> CreateElement([FromBody] ProductDto product) => Result.Failure(ErrorCodes.NotFound);

    [HttpPut]
    public async Task<Result> UpdateElement([FromBody] ProductDto product) => Result.Failure(ErrorCodes.NotFound);

    [HttpDelete("{productId}")]
    public async Task<Result> DeleteElement([FromRoute] ProductId productId) => Result.Failure(ErrorCodes.NotFound);

    private static async Task<QueryResult<TResult>> ExecuteQueryAsync<TData, TResult>(
        Func<Task<QueryResult<TData>>> action,
        Func<TData, TResult> converter,
        TData? fallbackData = null)
        where TResult : class
        where TData : class
    {
        var result = await action();

        if (!result.IsSuccess)
        {
            return (QueryResult<TResult>)QueryResult<TResult>.Failure(result.ErrorCode);
        }

        var data = result.Data ?? fallbackData;
        if (data == null)
        {
            return (QueryResult<TResult>)QueryResult<TResult>.Failure(ErrorCodes.NotFound);
        }

        return QueryResult<TResult>.Success(converter(data));
    }
}
