using System.Runtime.Serialization;
using OWP.OtherSport.Validation.Constants;

namespace OWP.OtherSport.Validation.Exceptions;

[Serializable]
public class DataLengthExceedsMaximumException : CriticalValidationException
{
    public DataLengthExceedsMaximumException() : base($"Excel data exceeds {ExcelDataLength.MaximumDataLength} records")
    {
    }

#if NET8_0_OR_GREATER
    [Obsolete(DiagnosticId = "SYSLIB0051")]
#endif
    protected DataLengthExceedsMaximumException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}