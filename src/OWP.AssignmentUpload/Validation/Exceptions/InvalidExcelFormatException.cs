namespace OWP.AssignmentUpload.Validation.Exceptions;

public class InvalidExcelFormatException : CriticalValidationException
{
    public InvalidExcelFormatException() : base("Incorrect template")
    {
    }

}