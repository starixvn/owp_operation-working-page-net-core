namespace OWP.OtherSport.Validation.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidExcelFormatException : CriticalValidationException
    {
        public InvalidExcelFormatException(string message) : base(message)
        {
        }

        protected InvalidExcelFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}