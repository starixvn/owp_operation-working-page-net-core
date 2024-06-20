using System.Runtime.Serialization;

namespace OWP.OtherSport.Validation.Exceptions;

[Serializable]
public class InvalidExcelCellFormatException : CriticalValidationException
{
    public InvalidExcelCellFormatException(string message) : base(message)
    {
    }

#if NET8_0_OR_GREATER
    [Obsolete(DiagnosticId = "SYSLIB0051")] 
#endif
    protected InvalidExcelCellFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}