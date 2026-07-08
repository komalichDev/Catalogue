using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages;

public partial class DescriptonCreator
{
    [Parameter]
    public required Description Description { get; set; }
}
