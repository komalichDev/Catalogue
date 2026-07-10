using BlazorBootstrap;
using Client.Helpers;
using Common.Types;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages;

public partial class Component
{
    private QueryResult<List<ProductDto>>? _produktListe;
    private string _errorMessage = string.Empty;
    private ProductDto? _selectedProduct = null;
    private Modal _productModal = default!;

    [Inject]
    protected HttpClient Http { get; set; } = default!;

    protected override async Task OnInitializedAsync()
        => _produktListe = await HttpRequestExecuter.ExecuteGetRequests<List<ProductDto>>(Http, "https://localhost:7053/api/Product");

    private void DeleteProduct(Shared.Models.ProductDto product)
    {
        // TODO: API Delete Call
    }

    private async Task ShowDetailedInfo(Shared.Models.ProductDto product)
    {
        _selectedProduct = product;

        var parameters = new Dictionary<string, object>
        {
            { "Id", _selectedProduct.Id },
            { "OnClose", EventCallback.Factory.Create(this, HandleEditorClosed) },
        };

        await _productModal.ShowAsync<ProductEditor>(
            title: "Produkt bearbeiten",
            parameters: parameters);
    }

    private async void HandleEditorClosed()
    {
        await _productModal.HideAsync();
        _selectedProduct = null;
    }
}
