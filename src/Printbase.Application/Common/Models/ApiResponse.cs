using System.Net;

namespace Printbase.Application.Common.Models;

public class ApiResponse<T>
{
    public bool Succeeded { get; protected set; }
    public string Message { get; protected set; } = string.Empty;
    public T? Data { get; protected set; }
    public IList<string> Errors { get; protected set; } = new List<string>();
    public HttpStatusCode StatusCode { get; protected set; }
    
    public static ApiResponse<T> Success(T data, HttpStatusCode statusCode = HttpStatusCode.OK, string message = "")
    {
        return new ApiResponse<T>
        {
            Succeeded = true,
            Data = data,
            StatusCode = statusCode,
            Message = message
        };
    }
    
    public static ApiResponse<T> Fail(string error, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new ApiResponse<T>
        {
            Succeeded = false,
            Errors = new List<string> { error },
            StatusCode = statusCode
        };
    }
    
    public static ApiResponse<T> Fail(IList<string> errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new ApiResponse<T>
        {
            Succeeded = false,
            Errors = errors,
            StatusCode = statusCode
        };
    }
}