using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using OWP.AssignmentUpload.Validation.Constants;
using OWP.AssignmentUpload.Validation.Models;

namespace OWP.AssignmentUpload.Validation.Helper
{
    public static class ValidationHelper
    {
        public static bool CheckLeagueRule(MinorMatch minorMatch, string league)
            => ExecuteCondition(minorMatch.LeagueLike, minorMatch.LeagueNotLike, league);

        public static bool CheckHomeRule(MinorMatch minorMatch, string home)
             => ExecuteCondition(minorMatch.HomeLike, minorMatch.HomeNotLike, home);

        public static bool CheckAwayRule(MinorMatch minorMatch, string away)
             => ExecuteCondition(minorMatch.AwayLike, minorMatch.AwayNotLike, away);

        public static bool ExecuteCondition(string likePattern, string noneLikePattern, string parentContainer)
            => (string.IsNullOrEmpty(likePattern) || parentContainer.IndexOf(likePattern, StringComparison.CurrentCultureIgnoreCase) >= 0)
                && (string.IsNullOrEmpty(noneLikePattern) || parentContainer.IndexOf(noneLikePattern, StringComparison.CurrentCultureIgnoreCase) < 0);

        public static int CountMatchSameLeagueHomeAway(List<AssignmentModel> owpMatchs, AssignmentModel owpMatch)
            => owpMatchs.Count(a => string.Equals(a.Home, owpMatch.Home, StringComparison.CurrentCultureIgnoreCase) &&
                                    string.Equals(a.Away, owpMatch.Away, StringComparison.CurrentCultureIgnoreCase) &&
                                    string.Equals(a.League, owpMatch.League, StringComparison.CurrentCultureIgnoreCase));
        public static int CountMatchSameLeagueHomeAwayKOTime(List<AssignmentModel> owpMatchs, AssignmentModel owpMatch)
            => owpMatchs.Count(a => string.Equals(a.Home, owpMatch.Home, StringComparison.CurrentCultureIgnoreCase) &&
                                    string.Equals(a.Away, owpMatch.Away, StringComparison.CurrentCultureIgnoreCase) &&
                                    string.Equals(a.League, owpMatch.League, StringComparison.CurrentCultureIgnoreCase) &&
                                    string.Equals(a.KickOffTime, owpMatch.KickOffTime));

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

        public static List<AssignmentModel> RemoveMatchGenerateBySystem(List<AssignmentModel> owpMatchs)
            => owpMatchs.Where(o => !MinorBettypeConst.MinorBettypeIdGenerateBySystem
             .Exists(m => m == o.MinorBettypeId))?.ToList();
    }
}
