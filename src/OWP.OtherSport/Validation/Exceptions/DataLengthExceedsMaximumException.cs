namespace OWP.OtherSport.Validation.Exceptions;

using Constants;
using System.Runtime.Serialization;

[Serializable]
public class DataLengthExceedsMaximumException : CriticalValidationException
{
    public DataLengthExceedsMaximumException() : base($"Excel data exceeds {ExcelDataLength.MaximumDataLength} records")
    {
    }

    protected DataLengthExceedsMaximumException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}