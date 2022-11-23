namespace OWP.AssignmentUpload.Validation.Exceptions
{
    using System;

    [Serializable]
    public class CriticalValidationException : Exception
    {
        public CriticalValidationException(string message) : base(message)
        {
        }
    }
}
