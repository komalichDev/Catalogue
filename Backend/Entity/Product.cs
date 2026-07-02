namespace Backend.Entity;

public record Product(
    int Id,
    string? Name,
    int Price = 0,
    Description? Description = null,
    string? Category = null
);
