using BlazorBootstrap;
using Client.Services;
using Common.Exception;
using Common.Types;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages;

public partial class Component : BaseComponent
{
    private QueryResult<List<ProductDto>>? produktListe;
    private ProductDto? selectedProduct = null;
    private Modal productModal = default!;

    [Inject]
    private IHttpProductApi ProductApi { get; set; } = null!;

    public async Task LoadData()
    {
        var data = await this.ExecuteLoadAsync(() => this.ProductApi.LoadProducts());

        if (data != null)
        {
            this.produktListe = QueryResult<List<ProductDto>>.Success(data);
        }
        else if (string.IsNullOrEmpty(this._errorMessage))
        {
            this.produktListe = QueryResult<List<ProductDto>>.Failure(ErrorCodes.NoDataFound);
        }
    }

    protected override async Task OnInitializedAsync()
        => await this.LoadData();

    private async Task DeleteProduct(Shared.Models.ProductDto product)
    {
        bool success = await this.ExecuteActionAsync(() => this.ProductApi.DeleteProduct(product.Id));

        if (success)
        {
            await this.LoadData();
        }

        if (this.selectedProduct == product)
        {
            this.selectedProduct = null;
        }
    }

    private async Task ShowDetailedInfo(Shared.Models.ProductDto product)
    {
        this.selectedProduct = product;

        var parameters = new Dictionary<string, object>
        {
            { "Id", this.selectedProduct.Id },
            { "OnClose", EventCallback.Factory.Create(this, this.HandleEditorClosed) },
        };

        await this.productModal.ShowAsync<ProductEditor>(
            title: "Produkt bearbeiten",
            parameters: parameters);
    }

    private async void HandleEditorClosed()
    {
        await this.productModal.HideAsync();
        this.selectedProduct = null;
        await this.LoadData();
    }
}
