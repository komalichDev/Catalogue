using Common.Types;

namespace DatabaseAccess.Repositorymodel;
public record Product(
    ProductId Id, 
    string Name, 
    double Price, 
    DescriptionId DescriptionId,
    Description? Description,
    CategoryId CategoryId,
    Category? Category);