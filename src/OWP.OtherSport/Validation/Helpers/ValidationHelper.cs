namespace OWP.OtherSport.Validation.Helpers
{
    using OWP.OtherSport.Validation.Models;
    using System.Collections.Immutable;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;

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

        private static readonly ImmutableList<string> sports = ImmutableList<string>.Empty.AddRange(
                    new[]
                    {
                "Basketball",
                "Football",
                "Field Hockey",
                "Ice Hockey",
                "Darts",
                "Pool",
                "Table Tennis",
                "Volleyball",
                "Handball",
                "Netball",
                "Cricket",
                "Snooker",
                "Baseball",
                "Badminton",
                "Tennis",
                "Other Sports"
                    }
                );

        public static bool IsValidateSport(string sport)
        {
            if (string.IsNullOrEmpty(sport) || !sports.Any(x => x.Equals(sport, System.StringComparison.CurrentCultureIgnoreCase)))
            {
                return false;
            }

            return true;
        }

        public static bool MatchInfoEqualAssignment(OSMatchInfo matchInfo, AssignmentModelInExcel otherMatchInfo)
            => matchInfo.Home.Equals(otherMatchInfo.Home) && matchInfo.Away.Equals(otherMatchInfo.Away) && matchInfo.Sport.Equals(otherMatchInfo.Sport);

        public static bool LeagueInfoEqualAssignment(OSMatchInfo matchInfo, AssignmentModelInExcel otherMatchInfo)
            => matchInfo.League.Equals(otherMatchInfo.League) && matchInfo.DateTime.Equals(DateTime.FromOADate(double.Parse(otherMatchInfo.DateTime, CultureInfo.InvariantCulture)));

        public static bool EqualMatchAssignment(AssignmentModelInExcel assignment, AssignmentModelInExcel otherAssignment)
            => IsEquals(otherAssignment.Home.Replace("(N)", "").Trim(), assignment.Home.Replace("(N)", "").Trim()) &&
               IsEquals(otherAssignment.Away, assignment.Away) &&
               IsEquals(otherAssignment.Sport, assignment.Sport);

        public static bool EqualLeagueAssignment(AssignmentModelInExcel assignment, AssignmentModelInExcel otherAssignment)
            => IsEquals(otherAssignment.DateTime, assignment.DateTime) &&
                                                    IsEquals(otherAssignment.League, assignment.League);

        private static bool IsEquals(string source, string value)
        {
            return !string.IsNullOrWhiteSpace(source) &&
                source.Equals(value, StringComparison.OrdinalIgnoreCase);
        }
    }
}