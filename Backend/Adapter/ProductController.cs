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
    {
        _interactor = interactor;
    }

    [HttpGet]
    public async Task<QueryResult<List<ProductDto>>> GetProducts()
    {
        var result = await _interactor.GetAllProducts();
        if (!result.IsSuccess)
        {
            return QueryResult<List<ProductDto>>.Failure(result.ErrorCode);
        }

        return QueryResult<List<ProductDto>>.Success(result.Data ?? new List<ProductDto>());
    }

    [HttpGet("{productId}")]
    public async Task<QueryResult<List<ProductDto>>> GetProduct([FromRoute] ProductId product)
    {
        var result = await _interactor.GetProductById(product);
        if (!result.IsSuccess)
        {
            return QueryResult<List<ProductDto>>.Failure(result.ErrorCode);
        }

        if (result.Data == null)
        {
            return QueryResult<List<ProductDto>>.Failure(ErrorCodes.NotFound);
        }

        var resultList = new List<ProductDto> { result.Data };
        return QueryResult<List<ProductDto>>.Success(resultList);
    }

    [HttpPost]
    public async Task<Result> CreateElement([FromBody] ProductDto product)
    {
        //var result = _interactor.Execute(Requests.CreateElement, _requestmodelConverter.Convert(product));
        return Result.Failure(ErrorCodes.NotFound);
    }

    [HttpPut]
    public async Task<Result> UpdateElement([FromBody] ProductDto product)
    {
        //var result = _interactor.Execute(Requests.CreateElement, _requestmodelConverter.Convert(product));
        return Result.Failure(ErrorCodes.NotFound);
    }

    [HttpDelete("{productId}")]
    public async Task<Result> DeleteElement([FromRoute] ProductId productId)
    {
        //var result = _interactor.Execute(Requests.CreateElement, _requestmodelConverter.Convert(product));
        return Result.Failure(ErrorCodes.NotFound);
    }
}
