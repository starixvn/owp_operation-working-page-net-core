using System.Runtime.Serialization;

namespace OWP.AssignmentUpload.Validation.Exceptions
{
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
