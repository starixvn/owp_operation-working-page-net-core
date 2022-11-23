namespace OWP.AssignmentUpload.Validation.Exceptions
{
    [Serializable]
    public class InvalidExcelFormatException : CriticalValidationException
    {
        public InvalidExcelFormatException() : base("Incorrect template")
        {
        }
    }
}
