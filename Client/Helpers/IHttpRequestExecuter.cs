using Common.Exception;
using Common.Types;
using static System.Net.WebRequestMethods;

namespace Client.Helpers;

public interface IHttpRequestExecuter
{
    public Task<QueryResult<T>> ExecuteGetRequests<T>(string url)
        where T : class;

    public Task<Result> ExecutePostRequest<T>(string url, T data);

    public Task<Result> ExecutePutRequest<T>(string url, T data);

    public Task<Result> ExecuteDeleteRequest<T>(string url, T id);
}
