using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages;

public partial class ProductEditor
{
    [Parameter]
    public required ProductDto Product { get; set; }

    private void ChangeCategoryView()
    {
    }
}
