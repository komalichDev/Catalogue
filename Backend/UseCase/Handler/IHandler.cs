using Backend.UseCase.Interactor.Requestmodel;
using Backend.UseCase.Interactor.Requests;
using Backend.UseCase.Interactor.Responsemodel;

namespace Backend.UseCase.Handler;

public interface IHandler
{
    public Responsemodel Execute(Requests requests, Requestmodel model);
}
