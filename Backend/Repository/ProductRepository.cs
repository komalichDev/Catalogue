using Backend.Entity;
using Backend.Repository.Converter;
using Backend.UseCase.Interactor;

using DatabaseAccess;

namespace Backend.Repository;

public class ProductRepository : IProductGateway
{
    private IProductDatabaseAccess _databaseAccess;

    public ProductRepository(IProductDatabaseAccess databaseAccess)
    {
        _databaseAccess = databaseAccess;
    }

    public List<Product> GetAllProducts()
        => ProductConverter.Convert(_databaseAccess.GetAllProducts().Result);
}
