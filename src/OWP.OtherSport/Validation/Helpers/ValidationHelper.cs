namespace OWP.OtherSport.Validation.Helpers
{
  using OWP.OtherSport.Validation.Models;
  using System.Collections.Immutable;
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
  }
}