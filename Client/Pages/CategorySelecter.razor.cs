using Client.Helpers;
using Common.Types;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages;

public partial class CategorySelecter
{
    [Parameter]
    public CategoryId Id { get; set; }

    [Inject]
    protected HttpClient Http { get; set; } = default!;

    private List<Category>? Categories { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var result = await HttpRequestExecuter.ExecuteGetRequests<List<Category>>(Http, $"https://localhost:7053/api/Product/Category/");

        if (result.IsSuccess && result.Data != null)
        {
            Categories = result.Data;
        }
    }
}