using OWP.OtherSport.Validation.Models;

namespace OWP.OtherSport.Validation.Exceptions;

public class WarningDeleteException : Exception
{
    public List<AssignmentValidationRowResult> ValidationResults { get; set; }

    public WarningDeleteException(List<AssignmentValidationRowResult> validationResults) : base("")
    {
        ValidationResults = validationResults;
    }
}
