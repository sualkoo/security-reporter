namespace webapi.Utils
{
    public class StartDateValidationAttribute : DateRangeValidationAttribute
    {
        public StartDateValidationAttribute(string otherPropertyName) : base(otherPropertyName)
        {
        }
    }
}
