using System.Runtime.Serialization;

namespace OWP.AssignmentUpload.Validation.Exceptions;

[Serializable]
public class DuplicatedTraderException : CriticalValidationException
{
    public DuplicatedTraderException(string traders) : base($"{traders} has already been used by someone else. Please check again!")
    {
    }

//#if net7.0_OR_GREATER
//    [Obsolete(DiagnosticId = "SYSLIB0051")]
//#endif
    protected DuplicatedTraderException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}