namespace Backend.UseCase.Interactor;

public interface IRequestHandler
{
    public Task<Responsemodel.Responsemodel> Execute(Requests.Requests request, Requestmodel.Requestmodel model);
}
