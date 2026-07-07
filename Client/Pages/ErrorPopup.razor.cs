using Microsoft.AspNetCore.Components;

namespace Client.Pages;

public partial class ErrorPopup : ComponentBase
{
    [Parameter]
    public bool IsVisible { get; set; }

    [Parameter]
    public string ErrorMessage { get; set; } = string.Empty;

    public void ShowError(string message)
    {
        ErrorMessage = message;
        IsVisible = true;
        StateHasChanged();
    }

    private void Close()
    {
        IsVisible = false;
        StateHasChanged();
    }
}
