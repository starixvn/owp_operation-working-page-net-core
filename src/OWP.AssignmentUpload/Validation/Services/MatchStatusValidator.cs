using OWP.AssignmentUpload.Validation.Constants;
using OWP.AssignmentUpload.Validation.Exceptions;
using OWP.AssignmentUpload.Validation.Helper;
using OWP.AssignmentUpload.Validation.Models;
using System.ComponentModel.DataAnnotations;

namespace OWP.AssignmentUpload.Validation.Services;

public interface MatchStatusValidator
{
    void Validate(List<AssignmentModel> assignments);
}

public class MatchStatusValidatorImp : MatchStatusValidator
{
    public void Validate(List<AssignmentModel> assignments)
    {
        var validationResults = new List<AssignmentValidationRowResult>();
        var inExcelAssignments = ValidationHelper.RemoveMatchGenerateBySystem(assignments);

        for (var i = 0; i < inExcelAssignments.Count; i++)
        {
            var rowIndex = i;
            var validationResultsOfAssignment = CheckMatchStatus(inExcelAssignments[i], inExcelAssignments);
            var validationResultsOfAssignmentRow = validationResultsOfAssignment
                .Select(v => new AssignmentValidationRowResult
                {
                    Row = rowIndex + 1,
                    Message = v.ErrorMessage
                });
            validationResults.AddRange(validationResultsOfAssignmentRow);
        }

        var hasError = validationResults.Any();
        if (hasError)
        {
            throw new InvalidAssignmentRowException(validationResults);
        }
    }

    private static IEnumerable<ValidationResult> CheckMatchStatus(AssignmentModel minorMatch, IEnumerable<AssignmentModel> matches)
    {
        var results = new List<ValidationResult>();
        if (string.IsNullOrEmpty(minorMatch.ParentHashKey))
        {
            return results;
        }

        var mainMatch = matches.FirstOrDefault(x => x.HashKey == minorMatch.ParentHashKey) ?? new AssignmentModel();
        if (mainMatch.MatchStatus is MatchStatus.Cancel or MatchStatus.Suspended or MatchStatus.Postponed &&
            minorMatch.MatchStatus != mainMatch.MatchStatus)
        {
            results.Add(new ValidationResult("The status of special events must be same as main match if main match is Cancel, Suspended, or Postponed"));
        }

        return results;
    }
}