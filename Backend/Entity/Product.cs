using Common.Types;

namespace Backend.Entity;

public record Product(
    ProductId Id,
    string Name,
    double Price,
    DescriptionId DescriptionId,
    Description Description,
    CategoryId CategoryId,
    Category Category);
