using System.Runtime.Serialization;

namespace OWP.AssignmentUpload.Validation.Exceptions;

[Serializable]
public class EventDateMismatchException : CriticalValidationException
{
    public EventDateMismatchException() : base("Date mismatch between excel file and selected date")
    {
    }

//#if net7.0_OR_GREATER
//    [Obsolete(DiagnosticId = "SYSLIB0051")]
//#endif
    protected EventDateMismatchException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}