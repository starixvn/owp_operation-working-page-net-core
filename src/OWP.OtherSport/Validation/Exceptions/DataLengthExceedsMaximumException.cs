namespace OWP.OtherSport.Validation.Exceptions
{
    using Constants;

    public class DataLengthExceedsMaximumException : CriticalValidationException
    {
        public DataLengthExceedsMaximumException() : base($"Excel data exceeds {ExcelDataLength.MaximumDataLength} records")
        {
        }
    }
}