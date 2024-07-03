using OWP.AssignmentUpload.Validation.Models;

namespace OWP.AssignmentUpload.Validation.Exceptions;


public class InvalidAssignmentRowException(List<AssignmentValidationRowResult> validationResults) : Exception("Some rows have invalid data")
{
    public List<AssignmentValidationRowResult> ValidationResults { get; set; }
}