namespace Shared.Models;

public record ProductDto(
    int Id,
    string Name,
    double Price,
    Description Description,
    Category Category);