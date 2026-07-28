using BlazorBootstrap;
using Client.Helpers;
using Client.Services;
using Common.Exception;
using Common.Types;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages;

public partial class ProductEditor
{
    private readonly IHttpProductApi _productApi;
    private Modal _categoryModal = default!;
    private string _editProductName = string.Empty;
    private double _editProductPrice = 0.0;

    private CategoryId _editCategoryId;
    private string _editShortSummary = string.Empty;
    private string _editDetailedText = string.Empty;
    private int _editWeight = 0;
    private CategorySelecter _categorySelecter = default!;
    private bool _isLoading = false;
    private string _errorMessage = string.Empty;

    private ProductDto? _product;

    public ProductEditor(IHttpProductApi productApi)
    {
        _productApi = productApi;
    }

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
            _errorMessage = string.Empty;
            await LoadData(Id);
            _isLoading = false;
        }

        StateHasChanged();
    }

    private async Task SaveData(ProductDto dto)
    {
        var result = Result.Failure(ErrorCodes.NoDataFound);

        if (dto.Id == 0)
        {
            result = await _productApi.AddProduct(dto);
        }
        else
        {
            result = await _productApi.UpdateProduct(dto);
        }

        if (!result.IsSuccess)
        {
            _errorMessage = result.ErrorCode.ToUserMessage();
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

    private async Task LoadData(ProductId id)
    {
        try
        {
            var result = await _productApi.LoadProduct(id);

            if (!result.IsSuccess)
            {
                _errorMessage = result.ErrorCode.ToUserMessage();
            }
            else
            {
                if (result.Data != null)
                {
                    _product = result.Data;
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
                else
                {
                    _errorMessage = "Produkt nicht gefunden.";
                }
            }
        }
        catch (Exception ex)
        {
            _errorMessage = $"Fehler beim Laden der Daten: {ex.Message}";
        }
    }

    private async Task HandleSave()
    {
        _errorMessage = string.Empty;
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

            await SaveData(updatedProduct);

            if (string.IsNullOrEmpty(_errorMessage))
            {
                await OnClose.InvokeAsync();
            }
        }
    }

    private void HandleCategoryChanged(CategoryId newId)
    {
        _editCategoryId = newId;
    }

    private async Task OnCategoryModalClosed()
    {
        if (_categorySelecter != null)
        {
            await _categorySelecter.LoadData();
        }
    }
}
