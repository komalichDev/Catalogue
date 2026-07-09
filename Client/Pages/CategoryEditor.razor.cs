using Client.Helpers;
using Common.Types;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages;

public partial class CategoryEditor
{
    private QueryResult<List<Category>>? _categoryListe;
    private string _errorMessage = string.Empty;
    private Category? _selectedCategory = null;
    private string _editName = string.Empty;

    [Inject]
    protected HttpClient Http { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _categoryListe = await HttpRequestExecuter.ExecuteGetRequests<List<Category>>(Http, $"https://localhost:7053/api/Product/Category/");
        }
        catch (Exception ex)
        {
            _errorMessage = $"Fehler beim Laden der Daten: {ex.Message}";
        }
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

        if (_categoryListe?.Data == null)
        {
            _categoryListe = QueryResult<List<Category>>.Success(new List<Category>());
        }

        _categoryListe.Data.Insert(0, newCategory);
        _selectedCategory = newCategory;
        _editName = string.Empty;
    }

    private async Task SaveAndClose(Category oldCategory)
    {
        // TODO: Hier sendest du später den POST oder PUT Request an deine API

        var updatedCategory = new Category(oldCategory.Id, _editName);

        if (_categoryListe?.Data != null)
        {
            var index = _categoryListe.Data.IndexOf(oldCategory);
            if (index != -1)
            {
                _categoryListe.Data[index] = updatedCategory;
            }
        }

        _selectedCategory = null;
    }

    private async Task DeleteCategory(Category category)
    {
        // TODO: API Delete Call

        _categoryListe?.Data?.Remove(category);

        if (_selectedCategory == category) 
        {
            _selectedCategory = null;
        }

        StateHasChanged();
    }
}