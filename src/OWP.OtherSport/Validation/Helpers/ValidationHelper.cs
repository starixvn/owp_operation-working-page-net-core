namespace OWP.OtherSport.Validation.Helpers
{
    using OWP.OtherSport.Validation.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
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
                       Row = rowIndex + 1,
                       Message = v.ErrorMessage
                   });
            validationRowResult.AddRange(validationResultsRow);
        }
    }
}
