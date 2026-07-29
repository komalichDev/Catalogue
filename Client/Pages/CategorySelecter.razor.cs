using Client.Services;
using Common.Types;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages;

public partial class CategorySelecter(IHttpProductApi productApi) : BaseComponent
{
    private readonly IHttpProductApi productApi = productApi;

    [Parameter]
    public CategoryId Id { get; set; }

    [Parameter]
    public EventCallback<CategoryId> IdChanged { get; set; }

    [Inject]
    protected HttpClient Http { get; set; } = default!;

    private List<Category>? Categories { get; set; }

    public async Task LoadData()
    {
        var data = await this.ExecuteLoadAsync(() => this.productApi.LoadCategories());
        if (data != null)
        {
            this.Categories = data;
        }
    }

    protected override async Task OnInitializedAsync()
        => await this.LoadData();

    private async Task OnCategoryChanged(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newId))
        {
            var categoryId = CategoryId.From(newId);
            await this.IdChanged.InvokeAsync(categoryId);
        }
    }
}