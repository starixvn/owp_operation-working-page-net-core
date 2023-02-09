namespace OWP.AssignmentUpload.Validation.Exceptions;

using System.Runtime.Serialization;

[Serializable]
public class InvalidDateTimeException : CriticalValidationException
{
    public InvalidDateTimeException() : base("Date is not in the correct format")
    {
    }

    protected InvalidDateTimeException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}