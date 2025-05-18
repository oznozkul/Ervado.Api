namespace Ervado.Application.Common.Models;

public class Response<T>
{
    public T? Data { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool Succeeded { get; set; }
    public List<string> Errors { get; set; } = new();

    public static Response<T> Success(T data, string message = "")
    {
        return new Response<T>
        {
            Data = data,
            Message = message,
            Succeeded = true
        };
    }

    public static Response<T> Failure(string error)
    {
        return new Response<T>
        {
            Succeeded = false,
            Errors = new List<string> { error }
        };
    }

    public static Response<T> Failure(List<string> errors)
    {
        return new Response<T>
        {
            Succeeded = false,
            Errors = errors
        };
    }
}

public class Response
{
    public string Message { get; set; } = string.Empty;
    public bool Succeeded { get; set; }
    public List<string> Errors { get; set; } = new();

    public static Response Success(string message = "")
    {
        return new Response
        {
            Message = message,
            Succeeded = true
        };
    }

    public static Response Failure(string error)
    {
        return new Response
        {
            Succeeded = false,
            Errors = new List<string> { error }
        };
    }

    public static Response Failure(List<string> errors)
    {
        return new Response
        {
            Succeeded = false,
            Errors = errors
        };
    }
} 