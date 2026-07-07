using Common.Types;

namespace DatabaseAccess.RepositoryModel;

public record Description(
    DescriptionId Id,
    string ShortSummary,
    string DetailedText,
    int WeightInGrams);
