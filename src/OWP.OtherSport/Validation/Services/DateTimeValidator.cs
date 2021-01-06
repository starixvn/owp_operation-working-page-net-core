namespace OWP.OtherSport.Validation.Services
{
    using OWP.OtherSport.Validation.Helpers;
    using OWP.OtherSport.Validation.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    public interface DateTimeValidator
    {
        List<AssignmentValidationRowResult> Validate(List<AssignmentModelInExcel> assignments);

        List<ValidationResult> CustomValidateDateTime(AssignmentModelInExcel assignment);
    }

    public class DateTimeValidatorImp : DateTimeValidator
    {
        public List<AssignmentValidationRowResult> Validate(List<AssignmentModelInExcel> assignments)
        {
            var validationResults = new List<AssignmentValidationRowResult>();

            for (var i = 0; i < assignments.Count; i++)
            {
                var rowIndex = i;           
                var validationResultsOfAssignment = CustomValidateDateTime(assignments[i]);

                ValidationHelper.BuildValidationRowResult(
                   validationResultsOfAssignment,
                   validationResults,
                   rowIndex);
            }

            return validationResults;
        }

        public List<ValidationResult> CustomValidateDateTime(AssignmentModelInExcel assignment)
        {
            var result = new List<ValidationResult>();

            const string regex = @"(?i:^(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\s*\d{2}\,\s*\d{4}\s*(1[0-2]|0?[1-9]):([0-5][0-9])\s*([AP][M])$)";
            var match = Regex.Match(assignment.DateTime, regex, RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                result.Add(new ValidationResult("[Column: Date Time] - Error: Invalid date time format"));
                return result;
            }

            try
            {
                DateTime.Parse(assignment.DateTime);
            }

            catch (FormatException ex)
            {
                result.Add(new ValidationResult($"[Column: Date Time] - Error: {ex.Message}"));
            }

            return result;
        }

    }
}
