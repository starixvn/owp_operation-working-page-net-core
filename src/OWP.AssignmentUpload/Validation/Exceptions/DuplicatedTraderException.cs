using System.Runtime.Serialization;

namespace OWP.AssignmentUpload.Validation.Exceptions;

[Serializable]
public class DuplicatedTraderException : CriticalValidationException
{
    public DuplicatedTraderException(string traders) : base($"{traders} has already been used by someone else. Please check again!")
    {
    }

    protected DuplicatedTraderException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}