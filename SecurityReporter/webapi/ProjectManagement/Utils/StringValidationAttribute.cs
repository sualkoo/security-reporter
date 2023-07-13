using System.ComponentModel.DataAnnotations;
namespace webapi.Utils;

public class StringValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null)
        {
            return true; // Allow null values
        }

        if (value is string stringValue)
        {
            return !string.IsNullOrWhiteSpace(stringValue);
        }

        return true; // Non-string values are considered valid
    }
}

