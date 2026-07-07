using System.Text.Json.Serialization;
using Common.Exception;

namespace Common.Types;

public class QueryResult<T>
{
    public T? Data { get; }
    public ErrorCodes ErrorCode { get; }

    public bool IsSuccess => ErrorCode == ErrorCodes.None;

    [JsonConstructor]
    private QueryResult(T? data, ErrorCodes errorCode)
    {
        Data = data;
        ErrorCode = errorCode;
    }

    public static QueryResult<T> Success(T data) => new QueryResult<T>(data, ErrorCodes.None);
    public static QueryResult<T> Failure(ErrorCodes error) => new QueryResult<T>(default!, error);
}
