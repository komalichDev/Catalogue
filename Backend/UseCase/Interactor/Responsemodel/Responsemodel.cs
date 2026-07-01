namespace Backend.UseCase.Interactor.Responsemodel;

public struct Responsemodel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int Price { get; set; }

    public Description Description { get; set; }

    public string Category { get; set; }

    public bool IsSucess { get; set; }
}
