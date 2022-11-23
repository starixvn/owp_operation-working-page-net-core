using System.Runtime.Serialization;

namespace OWP.AssignmentUpload.Validation.Exceptions
{
    [Serializable]
    public class InvalidDateTimeException : CriticalValidationException
    {
        public InvalidDateTimeException() : base("Date is not in the correct format")
        {
        }

        protected InvalidDateTimeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
