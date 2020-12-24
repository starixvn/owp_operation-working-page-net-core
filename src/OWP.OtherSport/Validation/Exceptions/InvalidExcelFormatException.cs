namespace OWP.OtherSport.Validation.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidExcelFormatException : CriticalValidationException
    {
        public InvalidExcelFormatException(string validHeader) : base($"Invalid header, should be {validHeader}")
        {
        }

        protected InvalidExcelFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}