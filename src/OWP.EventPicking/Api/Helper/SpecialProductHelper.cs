using Newtonsoft.Json;
using OWP.Constants;
using static System.DateTime;

namespace OWP.EventPicking.Api.Helper;

public static class SpecialProductHelper
{
    private const int SpreadPercentageMultiplier = 100;
    private const int MaxbetKiloDivisor = 1000;
    public static void BuildSpecialProduct<T>(IList<T> events) where T : class, EventProductCommon
    {
        foreach (var @event in events)
        {
            var (leaguePostfix, teamPostfix) = BuildPostFixSpecialProduct(@event.MinorBettypeId);
            @event.LeagueName = $"{@event.LeagueName} {leaguePostfix}";
            @event.HomeName = $"{@event.HomeName} {teamPostfix}";
            @event.AwayName = $"{@event.AwayName} {teamPostfix}";
        }
    }

    public static List<T> CombineMainAndChildMatchAndAddMatchNo<T>(IList<T> mainMatches, IList<T> childMatches) where T : class, EventProductCommon
    {
        var matches = new List<T>();

        var matchNo = 1;
        foreach (var mainMatch in mainMatches)
        {
            var childMatchesOfThisMainMatch = childMatches.Where(childMatch => childMatch.InfoSiteMatchId == mainMatch.InfoSiteMatchId).ToList();
            AssignMatchNo(mainMatch, matchNo, childMatchesOfThisMainMatch);
            matchNo++;

            matches.Add(mainMatch);
            matches.AddRange(childMatchesOfThisMainMatch);
        }

        return matches;
    }

    private static void AssignMatchNo<T>(T mainMatch, int matchNo, List<T> childMatchesOfThisMainMatch) where T : class, EventProductCommon
    {
        mainMatch.MatchNo = MatchNoHelper.BuildMainMatchNo(matchNo);
        foreach (var childMatch in childMatchesOfThisMainMatch)
        {
            childMatch.MatchNo = MatchNoHelper.BuildChildMatchNo(matchNo, childMatch.MinorBettypeId);
        }

        childMatchesOfThisMainMatch.Sort((x, y) => string.CompareOrdinal(x.MatchNo, y.MatchNo));
    }

    public static (string leaguePostfix, string teamPostfix) BuildPostFixSpecialProduct(int? minorBettypeId)
    {
        return minorBettypeId switch
        {
            Bettype.NoOfCorner => ("- CORNERS", "- No.of Corners"),
            Bettype.NextCorner => ("- CORNERS", "- Next Corners"),
            Bettype.TotalBooking => ("- BOOKING", "- Total Bookings"),
            Bettype.NextBooking => ("- BOOKING", "- Next Booking"),
            _ => (string.Empty, string.Empty)
        };
    }

    public static bool CheckMatchHaveBeenExclude(string homeName)
        => !string.IsNullOrEmpty(homeName) && (homeName.IsContains("(ET)") || homeName.IsContains("(PEN)"));

    public static string GetCleanedTeamPostfix(int? minorBettypeId)
    {
        var (_, teamPostfix) = BuildPostFixSpecialProduct(minorBettypeId);
        return string.IsNullOrEmpty(teamPostfix)
            ? teamPostfix
            : teamPostfix.TrimStart('-', ' ').Trim();
    }

    public static LoggedChangeModel CreateLoggedChangeModel(
        int infoSiteMatchId,
        string modifiedBy,
        string action,
        string oldValue = null,
        string newValue = null)
    {
        return new LoggedChangeModel
        {
            InfoSiteMatchId = infoSiteMatchId,
            UpdateTypeId = null,
            ModifiedBy = modifiedBy,
            Action = action,
            OldValue = oldValue,
            NewValue = newValue,
            UpDateTime = Now.ToGmt8(),
        };
    }

    public static async Task SaveEventLogAsync(Repository repository, List<LoggedChangeModel> logData, CancellationToken cancellationToken = default)
    {
        if (logData is not { Count: > 0 })
        {
            return;
        }

        var xml = XmlConverter.Serialize(logData);
        var criteria = new SaveEditEventLogsCriteria { Xml = xml };
        await repository.SaveEventLog(criteria).ConfigureAwait(false);
    }

    public static string FormatSpread(decimal? spread)
        => spread.HasValue ? $"{Math.Round(spread.Value * SpreadPercentageMultiplier)}%" : string.Empty;

    public static string FormatMaxbet(decimal? maxbet)
        => maxbet.HasValue ? $"{Math.Round(maxbet.Value / MaxbetKiloDivisor)}K" : string.Empty;

    /// <summary>
    /// Parse JSON string mapping of ParentId to existing MinorBettypeIds
    /// </summary>
    /// <param name="existingMinorBettypeIdsJson">JSON format: {"parentId1": "1,2", "parentId2": "3,4"}</param>
    /// <returns>Dictionary mapping parentId to list of existing MinorBettypeIds</returns>
    public static Dictionary<int, List<int>> ParseExistingMinorBettypeIds(string existingMinorBettypeIdsJson)
    {
        var result = new Dictionary<int, List<int>>();

        if (string.IsNullOrWhiteSpace(existingMinorBettypeIdsJson))
        {
            return result;
        }

        try
        {
            var mapping = JsonConvert.DeserializeObject<Dictionary<string, string>>(existingMinorBettypeIdsJson);
            if (mapping != null)
            {
                foreach (var kvp in mapping)
                {
                    if (!int.TryParse(kvp.Key, out var parentId))
                    {
                        continue;
                    }

                    var minorIds = ParseCommaSeparatedInts(kvp.Value);
                    result[parentId] = minorIds;
                }
            }
        }
        catch (JsonException)
        {
            // Return empty dictionary if JSON deserialization fails
            // This is expected behavior when the JSON format is invalid
        }

        return result;
    }

    private static List<int> ParseCommaSeparatedInts(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return [];
        }

        return value.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .Where(x => int.TryParse(x, out _))
            .Select(int.Parse)
            .ToList();
    }

    /// <summary>
    /// Write log for adding special products - only logs for NEW MinorBettypeIds
    /// </summary>
    public static Task WriteLogForAddSpecialProduct(
        Repository repository,
        List<int> specialProductIds,
        List<int> parentIds,
        Dictionary<int, List<int>> existingMinorBettypeIdsMap,
        string userName,
        CancellationToken cancellationToken = default)
    {
        return WriteSpecialProductLog(
            repository,
            specialProductIds,
            parentIds,
            existingMinorBettypeIdsMap,
            userName,
            FilterNewMinorBettypeIds,
            CreateAddLogAction,
            cancellationToken);
    }

    /// <summary>
    /// Write log for removing special products - only logs for EXISTING MinorBettypeIds
    /// </summary>
    public static Task WriteLogForRemoveSpecialProduct(
        Repository repository,
        List<int> specialProductIds,
        List<int> parentIds,
        Dictionary<int, List<int>> existingMinorBettypeIdsMap,
        string userName,
        CancellationToken cancellationToken = default)
    {
        return WriteSpecialProductLog(
            repository,
            specialProductIds,
            parentIds,
            existingMinorBettypeIdsMap,
            userName,
            FilterExistingMinorBettypeIds,
            CreateRemoveLogAction,
            cancellationToken);
    }

#pragma warning disable S107
    private static async Task WriteSpecialProductLog(
        Repository repository,
        List<int> specialProductIds,
        List<int> parentIds,
        Dictionary<int, List<int>> existingMinorBettypeIdsMap,
        string userName,
        Func<List<int>, List<int>, List<int>> filterFunc,
        Func<int, string> actionFactory,
        CancellationToken cancellationToken)
#pragma warning restore S107
    {
        var logData = BuildLogDataForSpecialProducts(
            specialProductIds,
            parentIds,
            existingMinorBettypeIdsMap,
            userName,
            filterFunc,
            actionFactory);

        await SaveEventLogAsync(repository, logData, cancellationToken);
    }

    private static List<LoggedChangeModel> BuildLogDataForSpecialProducts(
        List<int> specialProductIds,
        List<int> parentIds,
        Dictionary<int, List<int>> existingMinorBettypeIdsMap,
        string userName,
        Func<List<int>, List<int>, List<int>> filterFunc,
        Func<int, string> actionFactory)
    {
        var logData = new List<LoggedChangeModel>();

        foreach (var parentId in parentIds)
        {
            var existingMinorBettypeIds = existingMinorBettypeIdsMap.GetValueOrDefault(parentId, new List<int>());
            var filteredMinorBettypeIds = filterFunc(specialProductIds, existingMinorBettypeIds);

            foreach (var minorBettypeId in filteredMinorBettypeIds)
            {
                var action = actionFactory(minorBettypeId);
                logData.Add(CreateLoggedChangeModel(parentId, userName, action));
            }
        }

        return logData;
    }

    private static List<int> FilterNewMinorBettypeIds(List<int> specialProductIds, List<int> existingMinorBettypeIds)
    {
        return specialProductIds.Where(id => !existingMinorBettypeIds.Contains(id)).ToList();
    }

    private static List<int> FilterExistingMinorBettypeIds(List<int> specialProductIds, List<int> existingMinorBettypeIds)
    {
        return specialProductIds.Where(existingMinorBettypeIds.Contains).ToList();
    }

    private static string CreateAddLogAction(int minorBettypeId)
    {
        var teamPostfix = GetCleanedTeamPostfix(minorBettypeId);
        return $"Add a new product <b>{teamPostfix}</b>";
    }

    private static string CreateRemoveLogAction(int minorBettypeId)
    {
        var teamPostfix = GetCleanedTeamPostfix(minorBettypeId);
        return $"Delete the product <b>{teamPostfix}</b>";
    }
}
