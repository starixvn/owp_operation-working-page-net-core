using System.Runtime.Serialization;

namespace OWP.AssignmentUpload.Validation.Exceptions;

[Serializable]
public class CriticalValidationException : Exception
{
    public CriticalValidationException(string message) : base(message)
    {
    }


#if NET8_0_OR_GREATER
    [Obsolete(DiagnosticId = "SYSLIB0051")] // add this attribute to the serialization ctor
#endif
    protected CriticalValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}