using BlazorBootstrap;
using Common.Types;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages;

public partial class Home
{
    private ProductDto? _newProduct;
    private Modal _productModal = default!;

    private async Task OpenProductEditorAsync()
    {
        _newProduct = new ProductDto(
                    Id: ProductId.From(0),
                    Name: string.Empty,
                    Price: 0.0,
                    DescriptionId: DescriptionId.From(0),
                    Description: new Description(DescriptionId.From(0), string.Empty, string.Empty, 0),
                    CategoryId: CategoryId.From(0),
                    Category: new Category (CategoryId.From(0), string.Empty));

        var parameters = new Dictionary<string, object>
        {
            { "Id", ProductId.From(0) },
            { "OnClose", EventCallback.Factory.Create(this, CloseEditor) },
        };

        await _productModal.ShowAsync<ProductEditor>(
            title: "Neues Produkt erstellen",
            parameters: parameters);
    }

    private async Task CloseEditor()
    {
        await _productModal.HideAsync();
        _newProduct = null;
    }
}
