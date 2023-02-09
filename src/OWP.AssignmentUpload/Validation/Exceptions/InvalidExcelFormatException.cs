using System.Runtime.Serialization;

namespace OWP.AssignmentUpload.Validation.Exceptions;

[Serializable]
public class InvalidExcelFormatException : CriticalValidationException
{
    public InvalidExcelFormatException() : base("Incorrect template")
    {
    }

    protected InvalidExcelFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}