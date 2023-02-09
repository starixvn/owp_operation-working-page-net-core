namespace OWP.OtherSport.Validation.Helpers;

using System.ComponentModel.DataAnnotations;
using Models;

public static class ValidationHelper
{
    public static void BuildValidationRowResult(
        List<ValidationResult> validationResults,
        List<AssignmentValidationRowResult> validationRowResult,
        int rowIndex)
    {
        var validationResultsRow = validationResults
            .Select(v => new AssignmentValidationRowResult
            {
                Row = rowIndex,
                Message = v.ErrorMessage
            });
        validationRowResult.AddRange(validationResultsRow);
    }
}