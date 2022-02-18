namespace OWP.OtherSport.Validation.Exceptions
{
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidExcelCellFormatException : CriticalValidationException
    {
        public InvalidExcelCellFormatException(string message) : base(message)
        {
        }

        protected InvalidExcelCellFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}