using Microsoft.AspNetCore.Components;

namespace Client.Pages;

public partial class DescriptonCreator
{
    [Parameter]
    public string ShortSummary { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<string> ShortSummaryChanged { get; set; }

    [Parameter]
    public string DetailedText { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<string> DetailedTextChanged { get; set; }

    [Parameter]
    public int WeightInGrams { get; set; }

    [Parameter]
    public EventCallback<int> WeightInGramsChanged { get; set; }

    private Task OnShortSummaryChanged(ChangeEventArgs e)
    {
        ShortSummary = e.Value?.ToString() ?? string.Empty;
        return ShortSummaryChanged.InvokeAsync(ShortSummary);
    }

    private Task OnDetailedTextChanged(ChangeEventArgs e)
    {
        DetailedText = e.Value?.ToString() ?? string.Empty;
        return DetailedTextChanged.InvokeAsync(DetailedText);
    }

    private Task OnWeightChanged(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int weight))
        {
            WeightInGrams = weight;
            return WeightInGramsChanged.InvokeAsync(WeightInGrams);
        }

        return Task.CompletedTask;
    }
}
