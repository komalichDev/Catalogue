using Common.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseAccess.Entity;

[Table("descriptions")]
public class Description
{
    [Key]
    public DescriptionId Id { get; init; }
    public string ShortSummary { get; init; }
    public string DetailedText { get; init; }
    public int WeightInGrams { get; init; }
}