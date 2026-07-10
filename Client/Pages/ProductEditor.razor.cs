using BlazorBootstrap;
using Client.Helpers;
using Common.Exception;
using Common.Types;
using Microsoft.AspNetCore.Components;
using Shared.Models;
using static System.Net.WebRequestMethods;

namespace Client.Pages;

public partial class ProductEditor
{
    private Modal _categoryModal = default!;
    private string _editProductName = string.Empty;
    private double _editProductPrice = 0.0;

    private bool _isLoading = false;
    private string _errorMessage = string.Empty;

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
            return;
        }

        _isLoading = true;
        _errorMessage = string.Empty;

        var result = await HttpRequestExecuter.ExecuteGetRequests<List<ProductDto>>(
            Http,
            $"https://localhost:7053/api/Product/Product/{Id.Value}");

        if (result.IsSuccess && result.Data != null && result.Data.Any())
        {
            _product = result.Data.First();
            _editProductName = _product.Name;
            _editProductPrice = _product.Price;
        }
        else
        {
            _errorMessage = result.ErrorCode.ToUserMessage();
        }

        _isLoading = false;
    }

    private async Task SaveData(ProductDto dto)
    {
        var result = Result.Failure(ErrorCodes.NoDataFound);

        if (dto.Id == 0)
        {
            result = await HttpRequestExecuter.ExecutePostRequest(Http, $"", dto);
        }
        else
        {
            result = await HttpRequestExecuter.ExecutePutRequest(Http, $"", dto);
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
}
