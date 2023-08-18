namespace webapi.ProjectSearch.Models;

public class ErrorResponse
{
    public ErrorResponse(string message, List<string>? details)
    {
        Message = message;
        Details = details;
    }

    public string Message { get; }
    public List<string>? Details { get; }
}