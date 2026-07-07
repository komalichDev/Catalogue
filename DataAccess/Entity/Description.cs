using Common.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseAccess.Entity;

[Table("descriptions")]
public class Description
{
    [Key]
    public DescriptionId Id { get; init; }
    public required string ShortSummary { get; init; }
    public required string DetailedText { get; init; }
    public int WeightInGrams { get; init; }
}