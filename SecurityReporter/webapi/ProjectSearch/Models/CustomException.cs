namespace webapi.ProjectSearch.Models;

public class CustomException : Exception
{
    public CustomException(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }

    public CustomException(int statusCode, string message, List<string> details) : base(message)
    {
        StatusCode = statusCode;
        Details = details;
    }

    public int StatusCode { get; }
    public List<string>? Details { get; }
}