using BlazorBootstrap;
using Client.Helpers;
using Client.Services;
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
    private IHttpProductApi ProductApi { get; set; } = null!;

    public async Task LoadData()
    {
        try
        {
            _produktListe = await ProductApi.LoadProducts();
            if (!_produktListe.IsSuccess)
            {
                _errorMessage = ErrorMessageMapper.ToUserMessage(_produktListe.ErrorCode);
            }

            StateHasChanged();
        }
        catch (Exception ex)
        {
            _errorMessage = $"Fehler beim Laden der Daten: {ex.Message}";
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }

    private async Task DeleteProduct(Shared.Models.ProductDto product)
    {
        var result = await ProductApi.DeleteProduct(product.Id);
        if (!result.IsSuccess)
        {
            _errorMessage = ErrorMessageMapper.ToUserMessage(result.ErrorCode);
        }
        else
        {
            await LoadData();
        }

        if (_selectedProduct == product)
        {
            _selectedProduct = null;
        }

        StateHasChanged();
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
        await LoadData();
    }
}
