namespace OWP.OtherSport.Validation.Exceptions
{
    public class InvalidExcelCellFormatException : CriticalValidationException
    {
        public InvalidExcelCellFormatException(string message) : base(message)
        {
        }
    }
}