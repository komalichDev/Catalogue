namespace Backend.DataAccess.Repository;

public interface IRepository
{
    public Task<Repositorymodel.Repositorymodel> GetAllProducts();
}
