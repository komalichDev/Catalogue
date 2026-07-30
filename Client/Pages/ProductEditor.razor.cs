using BlazorBootstrap;
using Client.Services;
using Common.Types;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages;

public partial class ProductEditor(IHttpProductApi productApi) : BaseComponent
{
    private readonly IHttpProductApi _productApi = productApi;
    private Modal _categoryModal = default!;
    private string _editProductName = string.Empty;
    private double _editProductPrice = 0.0;

    private CategoryId _editCategoryId;
    private string _editShortSummary = string.Empty;
    private string _editDetailedText = string.Empty;
    private int _editWeight = 0;
    private CategorySelecter _categorySelecter = default!;
    private bool _isLoading = false;

    private ProductDto? _product;

    [Parameter]
    public ProductId Id { get; set; }

    [Parameter]
    public EventCallback OnClose { get; set; }

    [Inject]
    protected HttpClient Http { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        if (Id.Value == 0)
        {
            _product = new ProductDto(ProductId.From(0), string.Empty, 0.0, DescriptionId.From(0), null, CategoryId.From(0), null);
            _editProductName = string.Empty;
            _editProductPrice = 0.0;

            _editCategoryId = CategoryId.From(0);
            _editShortSummary = string.Empty;
            _editDetailedText = string.Empty;
            _editWeight = 0;

            _isLoading = false;
        }
        else
        {
            _isLoading = true;
            await LoadData(Id);
            _isLoading = false;
        }
    }

    private async Task<bool> SaveData(ProductDto dto)
        => await ExecuteActionAsync(() =>
        dto.Id == 0 ? _productApi.AddProduct(dto) : _productApi.UpdateProduct(dto));

    private async Task OpenCategoryEditorModal()
        => await _categoryModal.ShowAsync<CategoryEditor>(title: "Kategorien verwalten");

    private async Task CancelEdit()
        => await OnClose.InvokeAsync();

    private async Task LoadData(ProductId id)
    {
        var product = await ExecuteLoadAsync(() => _productApi.LoadProduct(id));

        if (product != null)
        {
            _product = product;
            _editProductName = _product.Name;
            _editProductPrice = _product.Price;
            _editCategoryId = _product.CategoryId;

            if (_product.Description != null)
            {
                _editShortSummary = _product.Description.ShortSummary;
                _editDetailedText = _product.Description.DetailedText;
                _editWeight = _product.Description.WeightInGrams;
            }
        }
    }

    private async Task HandleSave()
    {
        if (_product != null)
        {
            var newDescription = new Shared.Models.Description(
                _product.DescriptionId,
                _editShortSummary,
                _editDetailedText,
                _editWeight);

            var updatedProduct = _product with
            {
                Name = _editProductName,
                Price = _editProductPrice,
                CategoryId = _editCategoryId,
                Category = null,
                Description = newDescription,
            };

            bool isSuccess = await SaveData(updatedProduct);

            if (isSuccess)
            {
                await OnClose.InvokeAsync();
            }
        }
    }

    private void HandleCategoryChanged(CategoryId newId)
        => _editCategoryId = newId;

    private async Task OnCategoryModalClosed()
    {
        if (_categorySelecter != null)
        {
            await _categorySelecter.LoadData();
        }
    }
}
