using Client.Services;
using Common.Exception;
using Common.Types;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages;

public partial class CategoryEditor(IHttpProductApi productApi) : BaseComponent
{
    private readonly IHttpProductApi _productApi = productApi;
    private QueryResult<List<Category>>? _categoryListe;
    private Category? _selectedCategory = null;
    private string _editName = string.Empty;

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

        bool success = await ExecuteActionAsync(() =>
            category.Id.Value == 0 ? _productApi.AddCategory(updatedCategory) : _productApi.UpdateCategory(updatedCategory));

        if (success)
        {
            await LoadData();
            _selectedCategory = null;
        }
    }

    private async Task DeleteCategory(Category category)
    {
        bool success = await ExecuteActionAsync(() => _productApi.DeleteCategory(category.Id));

        if (success)
        {
            await LoadData();
            if (_selectedCategory == category)
            {
                _selectedCategory = null;
            }
        }
    }

    private async Task LoadData()
    {
        var data = await ExecuteLoadAsync(() => _productApi.LoadCategories());

        if (data != null)
        {
            _categoryListe = QueryResult<List<Category>>.Success(data);
        }
        else if (!string.IsNullOrEmpty(ErrorMessage))
        {
            _categoryListe = QueryResult<List<Category>>.Failure(ErrorCodes.FailedConnection);
        }
    }
}