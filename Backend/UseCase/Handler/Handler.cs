using Backend.UseCase.Handler.Converter;
using Backend.UseCase.Interactor.Requestmodel;
using Backend.UseCase.Interactor.Requests;
using Backend.UseCase.Interactor.Responsemodel;
using DataAccess;

namespace Backend.UseCase.Handler;

public class Handler : IHandler
{
    private IRepository _repository;
    private IResponsemodelConverter _responsemodelConverter;

    public Handler(IRepository repository, IResponsemodelConverter responsemodelConverter)
    {
        _repository = repository;
        _responsemodelConverter = responsemodelConverter;
    }

    public Responsemodel Execute(Requests requests, Requestmodel model)
    {
        switch(requests)
        {
            case Requests.GetAllElements:
                return _responsemodelConverter.ConvertToResponsemodel(_repository.GetAllProducts().Result);
            case Requests.GetElement:
            case Requests.CreateElement:
            case Requests.UpdateElement:
            case Requests.DeleteElement:
            default:
                throw new ArgumentException("Invalid request type");
        }
    }
}
