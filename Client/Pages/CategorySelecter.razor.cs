using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages;

public partial class CategorySelector
{
    [Parameter]
    public required Category Category { get; set; }
}
