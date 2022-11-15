namespace OWP.AssignmentUpload.Validation.Exceptions
{
    public class InvalidDateTimeException : CriticalValidationException
    {
        public InvalidDateTimeException() : base("Date is not in the correct format")
        {
        }
    }
}
