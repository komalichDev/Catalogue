namespace Backend.DataAccess.Repositorymodel;

public struct Repositorymodel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int Price { get; set; }

    public Description Description { get; set; }

    public Category Category { get; set; }
}
