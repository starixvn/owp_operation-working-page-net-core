namespace OWP.AssignmentUpload.Validation.Exceptions;

using System;
using System.Runtime.Serialization;

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