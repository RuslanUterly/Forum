using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper;

public class Result
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public Result(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    public static Result Success(string message) => new Result(true, message);
    public static Result Failure(string message) => new Result(false, message);
}

public record Error(int StatusCode, string Message);
