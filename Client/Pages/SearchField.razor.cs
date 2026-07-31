using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages;

public partial class SearchField : BaseComponent
{
    private Modal _configModal = default!;

    private ProductSearchConfiguration _searchConfig = new();

    [Parameter]
    public EventCallback<ProductSearchConfiguration> OnSearch { get; set; }

    private async Task OpenConfigurationModal()
    {
        var parameters = new Dictionary<string, object>
        {
            { "CurrentConfig", _searchConfig },
            { "OnConfigSaved", EventCallback.Factory.Create<ProductSearchConfiguration>(this, ApplyConfiguration) },
        };

        await _configModal.ShowAsync<ProductTextSelection>(
            title: "Sucheinstellungen",
            parameters: parameters);
    }

    private async Task ApplyConfiguration(ProductSearchConfiguration updatedConfig)
    {
        _searchConfig = updatedConfig;
        await _configModal.HideAsync();
    }

    private async Task ExecuteSearch()
    {
        await OnSearch.InvokeAsync(_searchConfig);
    }
}