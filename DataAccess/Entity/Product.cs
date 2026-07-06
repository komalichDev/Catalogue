using Common.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseAccess.Entity;

[Table("products")]
public class Product
{
    [Key]
    public ProductId Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public double Price { get; init; }
    public CategoryId CategoryId { get; init; }
    [ForeignKey(nameof(CategoryId))]
    public Category? Category { get; init; }
    public DescriptionId DescriptionId { get; init; }
    [ForeignKey(nameof(DescriptionId))]
    public Description? Description { get; init; }
}
