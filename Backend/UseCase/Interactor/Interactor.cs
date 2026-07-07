using Backend.UseCase.Interactor.Converter;
using Common.Types;
using Shared.Models;

namespace Backend.UseCase.Interactor;

public class Interactor : IInteractor
{
    private IProductGateway _gateway;

    public Interactor(IProductGateway gateway)
    {
        _gateway = gateway;
    }

    public async Task<QueryResult<List<ProductDto>>> GetAllProducts()
    {
        var result = await _gateway.GetAllProducts();
        if (!result.IsSuccess)
        {
            return QueryResult<List<ProductDto>>.Failure(result.ErrorCode);
        }

        var products = result.Data ?? new List<Entity.Product>();
        return QueryResult<List<ProductDto>>.Success(ProductDtoConverter.Convert(products));
    }

    public async Task<QueryResult<ProductDto>> GetProductById(ProductId id)
    {
        var result = await _gateway.GetOneProduct(id);
        if (!result.IsSuccess)
        {
            return QueryResult<ProductDto>.Failure(result.ErrorCode);
        }

        var product = result.Data;
        if (product == null)
        {
            return QueryResult<ProductDto>.Failure(result.ErrorCode);
        }

        return QueryResult<ProductDto>.Success(ProductDtoConverter.Convert(product));
    }
}
