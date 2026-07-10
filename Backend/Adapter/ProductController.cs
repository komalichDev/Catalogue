using Backend.UseCase.Interactor;
using Common.Exception;
using Common.Types;
using Microsoft.AspNetCore.Http.HttpResults;
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
    public async Task<Result> CreateProduct([FromBody] ProductDto product)
        => await ExecuteOperationAsync(() => _interactor.CreateProduct(product));

    [HttpPost]
    public async Task<Result> CreateCategory([FromBody] Category category)
        => await ExecuteOperationAsync(() => _interactor.CreateCategory(category));

    [HttpPut]
    public async Task<Result> UpdateProduct([FromBody] ProductDto product)
        => await ExecuteOperationAsync(() => _interactor.UpdateProduct(product));

    [HttpPut]
    public async Task<Result> UpdateCategory([FromBody] Category category)
        => await ExecuteOperationAsync(() => _interactor.UpdateCategory(category));

    [HttpDelete("{productId}")]
    public async Task<Result> DeleteProduct([FromRoute] ProductDto product)
        => await ExecuteOperationAsync(() => _interactor.DeleteProduct(product));

    [HttpDelete("{categoryId}")]
    public async Task<Result> DeleteCategory([FromRoute] CategoryId categoryId) 
        => await ExecuteOperationAsync(() => _interactor.DeleteCategory(categoryId));

    private static async Task<Result> ExecuteOperationAsync(Func<Task<Result>> operation)
    {
        var result = await operation();

        if (!result.IsSuccess)
        {
            return Result.Failure(result.ErrorCode);
        }

        return result;
    }

    private static async Task<QueryResult<TResult>> ExecuteQueryAsync<TData, TResult>(
        Func<Task<QueryResult<TData>>> action,
        Func<TData, TResult> converter,
        TData? fallbackData = null)
        where TResult : class
        where TData : class
    {
        var result = await action();

        if (!result.IsSuccess || result.Data == null)
        {
            return (QueryResult<TResult>)QueryResult<TResult>.Failure(result.ErrorCode);
        }

        return QueryResult<TResult>.Success(converter(result.Data));
    }
}
