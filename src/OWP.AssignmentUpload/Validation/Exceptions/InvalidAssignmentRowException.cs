using System.Runtime.Serialization;
using OWP.AssignmentUpload.Validation.Models;

namespace OWP.AssignmentUpload.Validation.Exceptions;

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