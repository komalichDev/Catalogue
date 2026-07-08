using Client.Helpers;
using Common.Types;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages;

public partial class CategorySelecter
{
    [Parameter]

    public required CategoryId Id { get; set; }

    [Inject]
    protected HttpClient Http { get; set; } = default!;

    private QueryResult<Category>? Category { get; set; }

    protected override async Task OnInitializedAsync()
        => Category = await HttpRequestExecuter.ExecuteGetRequests<Category>(Http, $"https://localhost:7053/api/Category/{Id}");
}
