using Common.Types;

namespace Shared.Models;

public record Description(
    DescriptionId Id,
    string ShortSummary,
    string DetailedText,
    int WeightInGrams);