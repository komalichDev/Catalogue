using System;
using System.Collections.Generic;
using System.Text;
using Common.Exception;

namespace Common.Types;

public class Result
{    public ErrorCodes ErrorCode { get; }

    public bool IsSuccess => ErrorCode == ErrorCodes.None;

    protected Result(ErrorCodes errorCode)
    {
        ErrorCode = errorCode;
    }

    public static Result Success() => new Result(ErrorCodes.None);
    public static Result Failure(ErrorCodes error) => new Result(error);
}
