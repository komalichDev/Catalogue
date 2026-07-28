using Client.Helpers;
using Client.Services;
using Common.Types;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages;

public partial class CategorySelecter
{
    private readonly IHttpProductApi _productApi;
    private string _errorMessage = string.Empty;

    public CategorySelecter(IHttpProductApi productApi)
        => _productApi = productApi;

    [Parameter]
    public CategoryId Id { get; set; }

    [Parameter]
    public EventCallback<CategoryId> IdChanged { get; set; }

    [Inject]
    protected HttpClient Http { get; set; } = default!;

    private List<Category>? Categories { get; set; }

    public async Task LoadData()
    {
        try
        {
            var result = await _productApi.LoadCategories();
            if (!result.IsSuccess)
            {
                _errorMessage = ErrorMessageMapper.ToUserMessage(result.ErrorCode);
            }
            else
            {
                if (result.Data != null)
                {
                    Categories = result.Data;
                }
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

    private async Task OnCategoryChanged(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newId))
        {
            var categoryId = CategoryId.From(newId);
            await IdChanged.InvokeAsync(categoryId);
        }
    }
}