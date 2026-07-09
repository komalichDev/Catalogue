using Common.Types;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages;

public partial class CategoryCreator
{
    [Parameter]
    public required Category Category { get; set; }
}
