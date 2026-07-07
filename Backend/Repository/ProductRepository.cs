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

        var result = dbResult.Data ?? new DatabaseAccess.Repositorymodel.ProductRepositoryModel(new List<DatabaseAccess.Repositorymodel.Product>());
        return QueryResult<List<Product>>.Success(ProductConverter.Convert(result));
    }
}
