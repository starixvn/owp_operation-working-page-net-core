namespace OWP.OtherSport.Validation.Exceptions
{
    using Models;
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidAssignmentRowException : Exception
    {
        public InvalidAssignmentRowException(List<AssignmentValidationRowResult> validationResults) : base("Some rows have invalid data")
        {
            ValidationResults = validationResults;
        }

        protected InvalidAssignmentRowException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public List<AssignmentValidationRowResult> ValidationResults { get; set; }
    }
}