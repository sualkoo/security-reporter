using System.ComponentModel.DataAnnotations;

public class EmailListValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is List<string> stringList)
        {
            if (stringList.Count == 0)
            {
                return false; // Empty list is considered invalid
            }

            return stringList.All(s => IsValidEmail(s));
        }

        return true; // Non-list values are considered valid
    }

    private bool IsValidEmail(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        return new EmailAddressAttribute().IsValid(value);
    }
}


