using OWP.AssigneeSuggestionList.Models;
using OWP.Extensions;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;

namespace OWP.AssigneeSuggestionList.Services;

public interface CategorizeSabaMatchServices
{
    IEnumerable<AssignmentModel> CategorizeAndGroupSabaMatches(IEnumerable<AssignmentModel> matches);
}

public class CategorizeSabaMatchServicesImp : CategorizeSabaMatchServices
{
    public IEnumerable<AssignmentModel> CategorizeAndGroupSabaMatches(IEnumerable<AssignmentModel> matches)
    {
        var sabaMatches = matches.Where(x => IsSaba(x.LeagueName)).ToList();

        CategorizeSabaMatches(sabaMatches);
        GroupSaba5Mins(sabaMatches);
        GroupSabaMultipleGames(sabaMatches);

        return sabaMatches;
    }

    private static void CategorizeSabaMatches(List<AssignmentModel> sabaMatches)
    {
        sabaMatches.ForEach(x => { x.MatchType = GetSabaBetType(x); });
    }

    public static MatchType GetSabaBetType(AssignmentModel match)
        => match.LeagueName switch
        {
            var s when s.IsContains("PES ") && s.IsContains("5 Mins") => MatchType.Saba5Mins,
            var s when s.IsContains("6 Mins") => MatchType.SabaFutsal6Mins,
            var s when s.IsContains("M88 SABA") && s.IsContains("16 Mins") => MatchType.SabaM8816Mins,
            var s when s.IsContains("16 Mins") => MatchType.Saba16Mins,
            var s when s.IsContains("15 Mins") => MatchType.Saba15Mins,
            var s when s.IsContains("20 Mins") => MatchType.Saba20Mins,
            _ => MatchType.SabaOther,
        };

    public static void GroupSaba5Mins(List<AssignmentModel> sabaMatches)
    {
        var saba5Mins = sabaMatches.Where(x => x.MatchType == MatchType.Saba5Mins).ToList();
        sabaMatches.RemoveAll(x => x.MatchType == MatchType.Saba5Mins);

        var groupedSaba5Mins = saba5Mins.GroupBy(
            x => (
                x.UserId,
                x.HomeName,
                x.LeagueName,
                x.AwayName,
                x.EventDateTime.AddMinutes(-x.EventDateTime.Minute)
            ))
            .Select(y => y.OrderBy(x => x.EventDateTime).First());

        sabaMatches.AddRange(groupedSaba5Mins);
    }

    public static void GroupSabaMultipleGames(List<AssignmentModel> sabaMatches)
    {
        MatchType[] multipleGameTypes = { MatchType.Saba15Mins, MatchType.Saba16Mins, MatchType.Saba20Mins };

        var multipleSabaMatches = sabaMatches.Where(x => multipleGameTypes.Contains(x.MatchType));
        sabaMatches.RemoveAll(x => multipleGameTypes.Contains(x.MatchType));

        var groupedMultipleMatches = multipleSabaMatches.GroupBy(
            x => (
                x.UserId,
                x.EventDateTime
            ))
            .Select(y =>
            {
                var firstItem = y.OrderBy(x => x.EventDateTime).First();
                firstItem.MatchType = GetMultipleMatchType(y.Count(), firstItem.MatchType);

                return firstItem;
            });

        sabaMatches.AddRange(groupedMultipleMatches);
    }

    public static MatchType GetMultipleMatchType(int count, MatchType matchType)
        => (count, matchType) switch
        {
            (1, MatchType.Saba16Mins) => MatchType.Saba16Mins,
            (1, _) => MatchType.SabaLink1G,
            (2, _) => MatchType.SabaLink2G,
            (3, _) => MatchType.SabaLink3G,
            (4, _) => MatchType.SabaLink4G,
            _ => MatchType.SabaOther
        };

    public static bool IsSaba(string owpLeagueName) => owpLeagueName.IsContains("SABA ");
}
