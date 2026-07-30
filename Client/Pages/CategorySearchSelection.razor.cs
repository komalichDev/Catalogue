using Client.Services;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages;

public partial class CategorySearchSelection : BaseComponent
{
    [Parameter]
    public List<int> SelectedCategoryIds { get; set; } = new();

    [Parameter]
    public EventCallback<List<int>> OnConfigSaved { get; set; }

    [Inject]
    private IHttpProductApi ProductApi { get; set; } = default!;

    private List<Category>? Categories { get; set; }

    private List<int> TempSelectedIds { get; set; } = new();

    private bool IsAllChecked
        => Categories != null && Categories.Any() && Categories.All(c => TempSelectedIds.Contains(c.Id.Value));

    private bool IsIndeterminate
        => TempSelectedIds.Any() && !IsAllChecked;

    protected override async Task OnInitializedAsync()
    {
        TempSelectedIds = new List<int>(SelectedCategoryIds);

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
            TempSelectedIds = Categories.Select(c => c.Id.Value).ToList();
        }
        else
        {
            TempSelectedIds.Clear();
        }
    }

    private void OnCategoryChanged(int id, ChangeEventArgs e)
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