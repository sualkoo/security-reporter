using webapi.Enums;
namespace webapi.Models

{
    public class ValidationError
    {
        public string FieldName { get; set; }
        public string ErrorMessage { get; set; }
    }
}
