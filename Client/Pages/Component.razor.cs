using BlazorBootstrap;
using Client.Services;
using Common.Exception;
using Common.Types;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages;

public partial class Component : BaseComponent
{
    private QueryResult<List<ProductDto>>? _produktListe;
    private ProductDto? _selectedProduct = null;
    private Modal _productModal = default!;

    [Inject]
    private IHttpProductApi ProductApi { get; set; } = null!;

    public async Task LoadData()
    {
        var data = await ExecuteLoadAsync(() => ProductApi.LoadProducts());

        if (data != null)
        {
            _produktListe = QueryResult<List<ProductDto>>.Success(data);
        }
        else if (string.IsNullOrEmpty(_errorMessage))
        {
            _produktListe = QueryResult<List<ProductDto>>.Failure(ErrorCodes.NoDataFound);
        }
    }

    protected override async Task OnInitializedAsync()
        => await LoadData();

    private async Task DeleteProduct(Shared.Models.ProductDto product)
    {
        bool success = await ExecuteActionAsync(() => ProductApi.DeleteProduct(product.Id));

        if (success)
        {
            await LoadData();
        }

        if (_selectedProduct == product)
        {
            _selectedProduct = null;
        }
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
