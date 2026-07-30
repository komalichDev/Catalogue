using Common.Types;

namespace Shared.Models;

public record ProductSearchConfiguration
{
    public string SearchText { get; set; } = string.Empty;

    public bool SearchTitle { get; set; } = true;

    public bool SearchShortDescription { get; set; } = true;

    public bool SearchLongDescription { get; set; } = true;

    public bool SearchByCategory { get; set; } = false;

    public List<CategoryId> SelectedCategoryIds { get; set; } = new();

    public bool SearchByPrice { get; set; } = false;

    public double? MinPrice { get; set; }

    public double? MaxPrice { get; set; }

    public bool SearchByWeight { get; set; } = false;

    public double? MinWeight { get; set; }

    public double? MaxWeight { get; set; }
}
