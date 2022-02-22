namespace OWP.OtherSport.Validation.Helpers
{
    using OWP.OtherSport.Validation.Models;

    using System.ComponentModel.DataAnnotations;

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
                       Row = rowIndex + 2,
                       Message = v.ErrorMessage
                   });
            validationRowResult.AddRange(validationResultsRow);
        }
    }
}