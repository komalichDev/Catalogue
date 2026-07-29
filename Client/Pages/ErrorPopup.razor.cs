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
        this.ErrorMessage = message;
        this.IsVisible = true;
        this.StateHasChanged();
    }

    private void Close()
    {
        this.IsVisible = false;
        this.StateHasChanged();
    }
}
