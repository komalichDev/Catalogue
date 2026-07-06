using Common.Types;

namespace Shared.Models;

public record ProductDto(
    ProductId Id,
    string Name,
    double Price,
    DescriptionId DescriptionId,
    Description? Description,
    CategoryId CategoryId,
    Category? Category);