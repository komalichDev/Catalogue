using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseAccess.Entity;

[Table("categories")]
public class Category
{
    [Key]
    public CategoryId Id { get; init; }
    public string Name { get; init; }
}