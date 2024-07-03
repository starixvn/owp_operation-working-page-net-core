namespace OWP.AssignmentUpload.Validation.Exceptions;

public class EventDateMismatchException : CriticalValidationException
{
    public EventDateMismatchException() : base("Date mismatch between excel file and selected date")
    {
    }
}