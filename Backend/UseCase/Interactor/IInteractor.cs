namespace Backend.UseCase.Interactor;

public interface IInteractor
{
    public Task<Responsemodel.Responsemodel> Execute(Requests.Requests request, Requestmodel.Requestmodel model);
}
