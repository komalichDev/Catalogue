using Client.Helpers;
using Common.Types;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages;

public partial class DescriptonCreator
{
    private string _editShortSummary = string.Empty;
    private string _editDetailedText = string.Empty;
    private double _editWeight = 0;

    private bool _isLoading = false;
    private string _errorMessage = string.Empty;

    [Parameter]
    public DescriptionId Id { get; set; }

    [Inject]
    protected HttpClient Http { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        if (Id.Value == 0)
        {
            _editShortSummary = string.Empty;
            _editDetailedText = string.Empty;
            _editWeight = 0;
            return;
        }

        _isLoading = true;
        _errorMessage = string.Empty;

        var result = await HttpRequestExecuter.ExecuteGetRequests<Description>(
             Http,
             $"https://localhost:7053/api/Product/Description/{Id.Value}");

        if (result.IsSuccess && result.Data != null)
        {
            _editShortSummary = result.Data.ShortSummary;
            _editDetailedText = result.Data.DetailedText;
            _editWeight = result.Data.WeightInGrams;
        }
        else
        {
            _errorMessage = result.ErrorCode.ToUserMessage();
        }

        _isLoading = false;
    }
}
