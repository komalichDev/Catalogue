using DataAccess.Repositorymodel;

namespace DataAccess
{
    public interface IRepository
    {
        public Task<Repositorymodel.Repositorymodel> GetAllProducts();
    }
}
