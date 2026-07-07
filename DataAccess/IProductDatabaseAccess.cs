using Common.Types;
using DatabaseAccess.Repositorymodel;

namespace DatabaseAccess;

public interface IProductDatabaseAccess
{
    public Task<QueryResult<Repositorymodel.ProductRepositoryModel>> GetAllProducts();
}
