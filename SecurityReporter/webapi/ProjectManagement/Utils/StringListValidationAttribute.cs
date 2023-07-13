using System.ComponentModel.DataAnnotations;
public class StringListValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is List<string> stringList)
        {
            if (stringList.Count == 0)
            {
                return false; // Empty list is considered invalid
            }

            return stringList.All(s => !string.IsNullOrWhiteSpace(s));
        }

        return true; // Non-list values are considered valid
    }
}

