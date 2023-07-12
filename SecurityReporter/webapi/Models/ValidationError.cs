using webapi.Enums;
namespace webapi.Models

{
    public class ValidationError
    {
        public int FieldNumber { get; set; }
        public ErrorMessages ErrorMessage { get; set; }
    }
}
