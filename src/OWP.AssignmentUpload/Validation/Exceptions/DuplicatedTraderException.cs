namespace OWP.AssignmentUpload.Validation.Exceptions;

using System.Runtime.Serialization;

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