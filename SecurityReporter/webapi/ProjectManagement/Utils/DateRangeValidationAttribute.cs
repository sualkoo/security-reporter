﻿namespace webapi.Utils
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class DateRangeValidationAttribute : ValidationAttribute
    {
        protected readonly string _otherPropertyName;
        public DateRangeValidationAttribute(string otherPropertyName)
        {
            _otherPropertyName = otherPropertyName;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_otherPropertyName);
            var otherValue = property.GetValue(validationContext.ObjectInstance, null);

            if (!(value is DateOnly startDate) || !(otherValue is DateOnly endDate))
            {
                return new ValidationResult($"Invalid type for comparison.");
            }

            if(startDate == DateOnly.MinValue || endDate == DateOnly.MinValue)
            {
                return ValidationResult.Success;
            }

            if (startDate < endDate)
            {
                return new ValidationResult($"End Date must be greater than or equal to " + _otherPropertyName + ".");
            }
            return ValidationResult.Success;
        }
    }
}
