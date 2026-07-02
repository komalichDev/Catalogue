using Backend.UseCase.Handler;

namespace Backend.UseCase.Interactor;

public class Interactor : IInteractor
{
    private IHandler _handler;

    public Interactor(IHandler handler)
    {
        _handler = handler;
    }

    public Responsemodel.Responsemodel Execute(Requests.Requests request, Requestmodel.Requestmodel model)
        => _handler.Execute(request, model);
}
