using Common.Types;

namespace Backend.Entity;

public record Description(
    DescriptionId Id,
    string ShortSummary,
    string DetailedText,
    int WeightInGrams);
