namespace Backend.Entity;

public record Product(
    int Id,
    string Name,
    double Price,
    Description Description,
    Category Category);
