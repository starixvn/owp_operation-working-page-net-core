using OWP.OtherSport.Validation.Constants;

namespace OWP.OtherSport.Validation.Exceptions;


public class DataLengthExceedsMaximumException : CriticalValidationException
{
    public DataLengthExceedsMaximumException() : base($"Excel data exceeds {ExcelDataLength.MaximumDataLength} records")
    {
    }

}