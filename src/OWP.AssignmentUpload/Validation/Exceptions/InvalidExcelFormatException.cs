using System.Runtime.Serialization;

namespace OWP.AssignmentUpload.Validation.Exceptions;

[Serializable]
public class InvalidExcelFormatException : CriticalValidationException
{
    public InvalidExcelFormatException() : base("Incorrect template")
    {
    }

#if NET8_0_OR_GREATER
    [Obsolete("This constructor is obsolete due to using formatter-based serialization.", DiagnosticId = "SYSLIB0051")]
#endif
    protected InvalidExcelFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}