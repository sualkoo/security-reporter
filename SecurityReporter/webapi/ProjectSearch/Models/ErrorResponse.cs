namespace webapi.ProjectSearch.Models
{
    public class ErrorResponse
    {
        public string Message { get; }
        public List<string> details { get; }
        public ErrorResponse(string message, List<string>? details)
        {
            this.Message = message;
            this.details = details;
        }
    }
}
