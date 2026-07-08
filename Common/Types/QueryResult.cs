using System.Text.Json.Serialization;
using Common.Exception;

namespace Common.Types;

public sealed class QueryResult<T> : Result where T : class
{
    public T? Data { get; }

    [JsonConstructor]
    private QueryResult(T? data, ErrorCodes errorCode) : base(errorCode)
    {
        Data = data;
    }

    public static QueryResult<T> Success(T data) => new QueryResult<T>(data, ErrorCodes.None);

}