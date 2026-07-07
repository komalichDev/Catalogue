using Backend.Entity;
using Backend.Repository.Converter;
using Backend.UseCase.Interactor;
using Common.Exception;
using Common.Types;
using DatabaseAccess;

namespace Backend.Repository;

public class ProductRepository : IProductGateway
{
    private IProductDatabaseAccess _databaseAccess;

    public ProductRepository(IProductDatabaseAccess databaseAccess)
    {
        _databaseAccess = databaseAccess;
    }

    public async Task<QueryResult<List<Product>>> GetAllProducts()
    {
        var dbResult = await _databaseAccess.GetAllProducts();
        if (!dbResult.IsSuccess)
        {
            return QueryResult<List<Product>>.Failure(dbResult.ErrorCode);
        }

        var result = dbResult.Data ?? new DatabaseAccess.RepositoryModel.ProductRepositoryModel(new List<DatabaseAccess.RepositoryModel.Product>());
        return QueryResult<List<Product>>.Success(ProductConverter.Convert(result));
    }

    public async Task<QueryResult<Product>> GetProductById(ProductId id)
    {
        var dbResult = await _databaseAccess.GetProduct(id);
        if (!dbResult.IsSuccess)
        {
            return QueryResult<Product>.Failure(dbResult.ErrorCode);
        }

        if (dbResult.Data is null)
        {
            return QueryResult<Product>.Failure(ErrorCodes.NotFound);
        }

        return QueryResult<Product>.Success(ProductConverter.Convert(dbResult.Data));
    }
}
