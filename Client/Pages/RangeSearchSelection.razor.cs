using Microsoft.AspNetCore.Components;

namespace Client.Pages;

public partial class RangeSearchSelection : BaseComponent
{
    private double? _tempMin;
    private double? _tempMax;

    [Parameter]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public string Unit { get; set; } = string.Empty;

    [Parameter]
    public double DefaultMin { get; set; } = 0;

    [Parameter]
    public double DefaultMax { get; set; } = 100;

    [Parameter]
    public double? MinValue { get; set; }

    [Parameter]
    public double? MaxValue { get; set; }

    [Parameter]
    public EventCallback<(double? Min, double? Max)> OnConfigSaved { get; set; }

    protected override void OnParametersSet()
    {
        _tempMin = MinValue;
        _tempMax = MaxValue;
    }

    private async Task SaveConfig() 
        => await OnConfigSaved.InvokeAsync((_tempMin, _tempMax));
}