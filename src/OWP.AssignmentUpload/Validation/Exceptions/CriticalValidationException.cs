using System.Runtime.Serialization;

namespace OWP.AssignmentUpload.Validation.Exceptions;

[Serializable]
public class CriticalValidationException : Exception
{
    public CriticalValidationException(string message) : base(message)
    {
    }

    protected CriticalValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}