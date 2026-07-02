using DataAccess.Repositorymodel;

namespace Backend.UseCase.Handler.Converter;

public interface IRepositorymodelConverter
{
    public Repositorymodel ConvertToRepositorymodel(List<Product> products);
}
