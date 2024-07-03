using System.Runtime.Serialization;
using OWP.OtherSport.Validation.Models;

namespace OWP.OtherSport.Validation.Exceptions;


public class InvalidAssignmentRowException(List<AssignmentValidationRowResult> validationResults) : Exception("Some rows have invalid data")
{
    public List<AssignmentValidationRowResult> ValidationResults { get; set; } = validationResults;
}