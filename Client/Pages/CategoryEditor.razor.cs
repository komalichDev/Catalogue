using Client.Helpers;
using Client.Services;
using Common.Types;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages;

public partial class CategoryEditor
{
    private readonly IHttpProductApi _productApi;
    private QueryResult<List<Category>>? _categoryListe;
    private string _errorMessage = string.Empty;
    private Category? _selectedCategory = null;
    private string _editName = string.Empty;

    public CategoryEditor(IHttpProductApi productApi)
    {
        _productApi = productApi;
    }

    [Inject]
    protected HttpClient Http { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }

    private void ToggleExpand(Category category)
    {
        if (_selectedCategory == category)
        {
            _selectedCategory = null;
        }
        else
        {
            _selectedCategory = category;
            _editName = category.Name;
        }
    }

    private void AddNewCategory()
    {
        var newCategory = new Category(CategoryId.From(0), string.Empty);

        if (_categoryListe?.Data != null)
        {
            _categoryListe.Data.Insert(0, newCategory);
            _selectedCategory = newCategory;
            _editName = string.Empty;
        }
        else
        {
            _categoryListe = QueryResult<List<Category>>.Success(new List<Category>());
        }
    }

    private async Task SaveAndClose(Category category)
    {
        var updatedCategory = new Category(category.Id, _editName);
        Result result;

        if (category.Id.Value == 0)
        {
            result = await _productApi.AddCategory(updatedCategory);
        }
        else
        {
            result = await _productApi.UpdateCategory(updatedCategory);
        }

        if (!result.IsSuccess)
        {
            _errorMessage = ErrorMessageMapper.ToUserMessage(result.ErrorCode);
            return;
        }

        await LoadData();

        _selectedCategory = null;
    }

    private async Task DeleteCategory(Category category)
    {
        var result = await _productApi.DeleteCategory(category.Id);
        if (!result.IsSuccess)
        {
            _errorMessage = ErrorMessageMapper.ToUserMessage(result.ErrorCode);
        }
        else
        {
            await LoadData();
            StateHasChanged();
        }

        if (_selectedCategory == category)
        {
            _selectedCategory = null;
        }
    }

    private async Task LoadData()
    {
        try
        {
            _categoryListe = await _productApi.LoadCategories();
            if (!_categoryListe.IsSuccess)
            {
                _errorMessage = ErrorMessageMapper.ToUserMessage(_categoryListe.ErrorCode);
            }
        }
        catch (Exception ex)
        {
            _errorMessage = $"Fehler beim Laden der Daten: {ex.Message}";
        }
    }
}