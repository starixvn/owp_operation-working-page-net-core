using System.ComponentModel.DataAnnotations;

namespace OWP.AssignmentUpload.Validation.Annotations;

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

        if (!listString.Any(item => !string.IsNullOrWhiteSpace(item) && item.Length > MaximumLength))
        {
            return null;
        }

        var message = string.Format(ErrorMessage, MaximumLength);
        return new ValidationResult(message);

    }
}