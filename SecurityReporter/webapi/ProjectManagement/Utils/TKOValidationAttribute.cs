namespace webapi.Utils
{
    public class TKOValidationAttribute : DateRangeValidationAttribute
    {
        public TKOValidationAttribute(string otherPropertyName) : base(otherPropertyName)
        {
        }
    }
}