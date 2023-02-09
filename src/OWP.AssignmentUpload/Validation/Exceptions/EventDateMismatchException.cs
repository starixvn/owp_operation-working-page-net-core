using System.Runtime.Serialization;

namespace OWP.AssignmentUpload.Validation.Exceptions;

[Serializable]
public class EventDateMismatchException : CriticalValidationException
{
    public EventDateMismatchException() : base("Date mismatch between excel file and selected date")
    {
    }

    protected EventDateMismatchException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}