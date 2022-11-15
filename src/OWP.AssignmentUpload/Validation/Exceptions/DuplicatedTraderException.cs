namespace OWP.AssignmentUpload.Validation.Exceptions
{
    public class DuplicatedTraderException : CriticalValidationException
    {
        public DuplicatedTraderException(string traders) : base($"{traders} has already been used by someone else. Please check again!")
        {
        }
    }
}
