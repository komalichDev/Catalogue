using System.Net.Http.Json;
using Common.Exception;
using Common.Types;

namespace Client.Helpers;

public class HttpRequestExecuter(HttpClient http) : IHttpRequestExecuter
{
    public Task<QueryResult<T>> ExecuteGetRequests<T>(string url)
        where T : class
            => ExecuteSafelyAsync(
                async () =>
                    {
                        var value = await http.GetFromJsonAsync<QueryResult<T>>(url);
                        return value ?? QueryResult<T>.Failure(ErrorCodes.NetworkError);
                    },
                QueryResult<T>.Failure(ErrorCodes.NetworkError));

    public Task<Result> ExecutePostRequest<T>(string url, T data)
        => ExecuteSendRequestAsync(() => http.PostAsJsonAsync(url, data));

    public Task<Result> ExecutePutRequest<T>(string url, T data)
        => ExecuteSendRequestAsync(() => http.PutAsJsonAsync(url, data));

    public Task<Result> ExecuteDeleteRequest<T>(string url, T id)
        => ExecuteSafelyAsync(
            async () =>
                {
                    var value = await http.DeleteFromJsonAsync<Result>($"{url}/{id}");
                    if (value == null)
                    {
                        return Result.Failure(ErrorCodes.NetworkError);
                    }

                    return value.IsSuccess ? Result.Success() : Result.Failure(value.ErrorCode);
                },
            Result.Failure(ErrorCodes.NetworkError));

    private Task<Result> ExecuteSendRequestAsync(Func<Task<HttpResponseMessage>> requestAction)
    {
        return ExecuteSafelyAsync(
            async () =>
                {
                    using var response = await requestAction();
                    if (response.IsSuccessStatusCode)
                    {
                        var serverResult = await response.Content.ReadFromJsonAsync<Result>();
                        return serverResult ?? Result.Failure(ErrorCodes.NetworkError);
                    }

                    return Result.Failure(ErrorCodes.NetworkError);
                },
            Result.Failure(ErrorCodes.NetworkError));
    }

    private async Task<TResult> ExecuteSafelyAsync<TResult>(Func<Task<TResult>> action, TResult fallback)
    {
        try
        {
            return await action();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Netzwerkfehler: {ex.Message}");
            return fallback;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Allgemeiner Fehler: {ex.ToString()}");
            return fallback;
        }
    }
}
