using Client.Helpers;
using Client.Services;
using Common.Types;
using Microsoft.AspNetCore.Components;

namespace Client.Pages;

public partial class DescriptonCreator
{
    private readonly IHttpProductApi _productApi;
    private string _editShortSummary = string.Empty;
    private string _editDetailedText = string.Empty;
    private double _editWeight = 0;

    private bool _isLoading = false;
    private string _errorMessage = string.Empty;

    public DescriptonCreator(IHttpProductApi productApi)
    {
        _productApi = productApi;
    }

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

        await LoadData(Id);
        StateHasChanged();
        _isLoading = false;
    }

    private async Task LoadData(DescriptionId id)
    {
        try
        {
            var result = await _productApi.LoadDescription(id);

            if (!result.IsSuccess)
            {
                _errorMessage = result.ErrorCode.ToUserMessage();
            }
            else
            {
                if (result.Data != null)
                {
                    _editShortSummary = result.Data.ShortSummary;
                    _editDetailedText = result.Data.DetailedText;
                    _editWeight = result.Data.WeightInGrams;
                }
                else
                {
                    _errorMessage = result.ErrorCode.ToUserMessage();
                }
            }
        }
        catch (Exception ex)
        {
            _errorMessage = $"Fehler beim Laden der Daten: {ex.Message}";
        }
    }
}
