using Common.Types;

namespace DatabaseAccess.Repositorymodel;

public record Description(
    DescriptionId Id,
    string ShortSummary,
    string DetailedText,
    int WeightInGrams);
