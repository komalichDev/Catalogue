using DatabaseAccess.Repositorymodel;

namespace DatabaseAccess;

public interface IProductDatabaseAccess
{
    public Task<Repositorymodel.ProductRepositoryModel> GetAllProducts();
}
