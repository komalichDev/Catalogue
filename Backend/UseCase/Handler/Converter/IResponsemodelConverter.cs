using Backend.UseCase.Interactor.Responsemodel;
using DataAccess.Repositorymodel;

namespace Backend.UseCase.Handler.Converter;

public interface IResponsemodelConverter
{
    public Responsemodel ConvertToResponsemodel(Repositorymodel model);
}
