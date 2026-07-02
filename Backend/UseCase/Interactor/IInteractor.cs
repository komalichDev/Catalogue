namespace Backend.UseCase.Interactor;

public interface IInteractor
{
    public Responsemodel.Responsemodel Execute(Requests.Requests request, Requestmodel.Requestmodel model);
}
