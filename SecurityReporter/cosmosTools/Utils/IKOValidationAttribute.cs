namespace webapi.Utils
{
    public class IKOValidationAttribute : DateRangeValidationAttribute
    {
        public IKOValidationAttribute(string otherPropertyName) : base(otherPropertyName)
        {
        }
    }
}
