namespace OWP.AssignmentUpload.Validation.Services
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Constants;
    using Exceptions;
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

            for (var i = 0; i < assignments.Count; i++)
            {
                var rowIndex = i;
                var validationResultsOfAssignment = CheckMatchStatus(assignments[i], assignments);                
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
}
