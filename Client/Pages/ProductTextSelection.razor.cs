using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages;

public partial class ProductTextSelection : BaseComponent
{
    [Parameter]
    public ProductSearchConfiguration CurrentConfig { get; set; } = new();

    [Parameter]
    public EventCallback<ProductSearchConfiguration> OnConfigSaved { get; set; }

    private ProductSearchConfiguration TempConfig { get; set; } = new();

    private bool IsDescriptionAllChecked
        => TempConfig.SearchShortDescription && TempConfig.SearchLongDescription;

    private bool IsDescriptionIndeterminate
        => TempConfig.SearchShortDescription != TempConfig.SearchLongDescription;

    protected override void OnParametersSet()
        => TempConfig = CurrentConfig with { };

    private void OnMasterDescriptionChanged(ChangeEventArgs e)
    {
        bool isChecked = (bool)(e.Value ?? false);
        TempConfig.SearchShortDescription = isChecked;
        TempConfig.SearchLongDescription = isChecked;
    }

    private async Task SaveConfig()
        => await OnConfigSaved.InvokeAsync(TempConfig);
}