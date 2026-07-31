using Client.Services;
using Common.Types;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages;

public partial class CategorySearchSelection : BaseComponent
{
    [Parameter]
    public List<CategoryId> SelectedCategoryIds { get; set; } = new();

    [Parameter]
    public EventCallback<List<CategoryId>> OnConfigSaved { get; set; }

    [Inject]
    private IHttpProductApi ProductApi { get; set; } = default!;

    private List<Category>? Categories { get; set; }

    private List<CategoryId> TempSelectedIds { get; set; } = new();

    private bool IsAllChecked
        => Categories != null && Categories.Any() && Categories.All(c => TempSelectedIds.Contains(c.Id));

    private bool IsIndeterminate
        => TempSelectedIds.Any() && !IsAllChecked;

    protected override async Task OnInitializedAsync()
    {
        TempSelectedIds = new List<CategoryId>(SelectedCategoryIds);

        var data = await ExecuteLoadAsync(() => ProductApi.LoadCategories());
        if (data != null)
        {
            Categories = data;
        }
    }

    private void OnMasterChanged(ChangeEventArgs e)
    {
        bool isChecked = (bool)(e.Value ?? false);
        if (isChecked && Categories != null)
        {
            TempSelectedIds = Categories.Select(c => c.Id).ToList();
        }
        else
        {
            TempSelectedIds.Clear();
        }
    }

    private void OnCategoryChanged(CategoryId id, ChangeEventArgs e)
    {
        bool isChecked = (bool)(e.Value ?? false);
        if (isChecked && !TempSelectedIds.Contains(id))
        {
            TempSelectedIds.Add(id);
        }
        else if (!isChecked)
        {
            TempSelectedIds.Remove(id);
        }
    }

    private async Task SaveConfig()
        => await OnConfigSaved.InvokeAsync(TempSelectedIds);
}