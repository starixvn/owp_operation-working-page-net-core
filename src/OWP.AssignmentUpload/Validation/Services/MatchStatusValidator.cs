namespace OWP.AssignmentUpload.Validation.Services;

using System.ComponentModel.DataAnnotations;
using Constants;
using Exceptions;
using Helper;
using Models;

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

    private static List<ValidationResult> CheckMatchStatus(AssignmentModel minorMatch, List<AssignmentModel> matches)
    {
        var results = new List<ValidationResult>();
        if (string.IsNullOrEmpty(minorMatch.ParentHashKey))
        {
            return results;
        }

        var mainMatch = matches.FirstOrDefault(x => x.HashKey == minorMatch.ParentHashKey);
        if ((mainMatch.MatchStatus == MatchStatus.Cancel ||
             mainMatch.MatchStatus == MatchStatus.Suspended ||
             mainMatch.MatchStatus == MatchStatus.Postponed) &&
            minorMatch.MatchStatus != mainMatch.MatchStatus)
        {
            results.Add(new ValidationResult("The status of special events must be same as main match if main match is Cancel, Suspended, or Postponed"));
        }

        return results;
    }
}