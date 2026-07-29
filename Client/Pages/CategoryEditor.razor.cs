using Client.Services;
using Common.Exception;
using Common.Types;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages;

public partial class CategoryEditor(IHttpProductApi productApi) : BaseComponent
{
    private readonly IHttpProductApi productApi = productApi;
    private QueryResult<List<Category>>? categoryListe;
    private Category? selectedCategory = null;
    private string editName = string.Empty;

    [Inject]
    protected HttpClient Http { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await this.LoadData();
    }

    private void ToggleExpand(Category category)
    {
        if (this.selectedCategory == category)
        {
            this.selectedCategory = null;
        }
        else
        {
            this.selectedCategory = category;
            this.editName = category.Name;
        }
    }

    private void AddNewCategory()
    {
        var newCategory = new Category(CategoryId.From(0), string.Empty);

        if (this.categoryListe?.Data != null)
        {
            this.categoryListe.Data.Insert(0, newCategory);
            this.selectedCategory = newCategory;
            this.editName = string.Empty;
        }
        else
        {
            this.categoryListe = QueryResult<List<Category>>.Success(new List<Category>());
        }
    }

    private async Task SaveAndClose(Category category)
    {
        var updatedCategory = new Category(category.Id, this.editName);

        bool success = await this.ExecuteActionAsync(() =>
            category.Id.Value == 0 ? this.productApi.AddCategory(updatedCategory) : this.productApi.UpdateCategory(updatedCategory));

        if (success)
        {
            await this.LoadData();
            this.selectedCategory = null;
        }
    }

    private async Task DeleteCategory(Category category)
    {
        bool success = await this.ExecuteActionAsync(() => this.productApi.DeleteCategory(category.Id));

        if (success)
        {
            await this.LoadData();
            if (this.selectedCategory == category)
            {
                this.selectedCategory = null;
            }
        }
    }

    private async Task LoadData()
    {
        var data = await this.ExecuteLoadAsync(() => this.productApi.LoadCategories());

        if (data != null)
        {
            this.categoryListe = QueryResult<List<Category>>.Success(data);
        }
        else if (!string.IsNullOrEmpty(this._errorMessage))
        {
            this.categoryListe = QueryResult<List<Category>>.Failure(ErrorCodes.FailedConnection);
        }
    }
}