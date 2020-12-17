namespace OWP.OtherSport.Validation.Exceptions
{
    using System;

    public class CriticalValidationException : Exception
    {
        public CriticalValidationException(string message) : base(message)
        {
        }
    }
}