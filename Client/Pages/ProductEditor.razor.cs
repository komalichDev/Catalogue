using BlazorBootstrap;
using Common.Types;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages;

public partial class ProductEditor
{
    private bool _showCategoryCreator = false;

    private Category _newCategory = new Category(CategoryId.From(0), string.Empty);

    private Modal _categoryModal = default!;
    private string _editProductName = string.Empty;
    private double _editProductPrice = 0.0;

    [Parameter]
    public required ProductDto Product { get; set; }

    [Parameter]
    public EventCallback OnClose { get; set; }

    protected override void OnParametersSet()
    {
        if (Product != null)
        {
            _editProductName = Product.Name;
            _editProductPrice = Product.Price;
        }
    }

    private async Task OpenCategoryEditorModal()
    {
        await _categoryModal.ShowAsync<CategoryEditor>(title: "Kategorien verwalten");
    }

    private async Task CancelEdit()
    {
        await OnClose.InvokeAsync();
    }
}
