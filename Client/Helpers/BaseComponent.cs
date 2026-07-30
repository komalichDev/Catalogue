using Client.Helpers;
using Common.Types;
using Microsoft.AspNetCore.Components;

namespace Client.Pages;

public abstract class BaseComponent : ComponentBase
{
    protected string _errorMessage = string.Empty;

    protected async Task<T?> ExecuteLoadAsync<T>(Func<Task<QueryResult<T>>> apiCall)
        where T : class
    {
        _errorMessage = string.Empty;
        try
        {
            var result = await apiCall();
            if (!result.IsSuccess)
            {
                _errorMessage = ErrorMessageMapper.ToUserMessage(result.ErrorCode);
                return null;
            }

            return result.Data;
        }
        catch (Exception ex)
        {
            _errorMessage = $"Fehler beim Laden der Daten: {ex.Message}";
            return null;
        }
        finally
        {
            StateHasChanged();
        }
    }

    protected async Task<bool> ExecuteActionAsync(Func<Task<Result>> action)
    {
        _errorMessage = string.Empty;
        try
        {
            var result = await action();
            if (!result.IsSuccess)
            {
                _errorMessage = ErrorMessageMapper.ToUserMessage(result.ErrorCode);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _errorMessage = $"Ein Fehler ist aufgetreten: {ex.Message}";
            return false;
        }
        finally
        {
            StateHasChanged();
        }
    }
}