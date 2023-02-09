namespace OWP.AssignmentUpload.Validation.Anotations;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class StringLengthOfListStringAttribute : ValidationAttribute
{
    public StringLengthOfListStringAttribute(int maximumLength)
    {
        MaximumLength = maximumLength;
    }

    public int MaximumLength { get; }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return null;
        }

        var listString = (List<string>)value;

        if (!listString.Any())
        {
            return null;
        }

        foreach (var item in listString)
        {
            if (string.IsNullOrWhiteSpace(item) || item.Length <= MaximumLength)
            {
                continue;

            }

            var message = string.Format(ErrorMessage, MaximumLength);
            return new ValidationResult(message);
        }

        return null;
    }
}