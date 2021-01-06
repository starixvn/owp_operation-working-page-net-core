namespace OWP.OtherSport.Validation.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidExcelFormatException : CriticalValidationException
    {
        public InvalidExcelFormatException() : base("Incorrect template")
        {
        }

        protected InvalidExcelFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}