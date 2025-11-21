
using Fastenshtein;

namespace OWP.EventPicking.Api.Services;

public interface SyncEventPickingInfo
{
    Task Execute(List<EventPickingModel> matchFiltered);
}

public class SyncEventPickingInfoImp(Repository repository) : SyncEventPickingInfo
{
    public async Task Execute(List<EventPickingModel> matchFiltered)
    {
        var leagueSetting = (await repository.QueryLeagueSetting()).ToList();
        var tvList = (await repository.GetTVList()).ToList();

        foreach (var match in matchFiltered)
        {
            if (match.IsNoOffer == null)
            {
                var leagueSettingByLeagueId = leagueSetting.Where(x => x.OWPLeagueId == match.OWPLeagueId).ToList();
                ProcessLeagueSettingsForMatch(leagueSettingByLeagueId, match, tvList);
            }

        }
    }

    private void ProcessLeagueSettingsForMatch(List<LeagueSettingInfoSender> leagueSettingByLeagueId, EventPickingModel match, List<Condition> tvList)
    {
        if (leagueSettingByLeagueId.Count > 0)
        {
            var selectedSetting = HandleMappingForMultipleSettings(match, leagueSettingByLeagueId, tvList);
            if (selectedSetting != null)
            {
                SuggestionValue(match, selectedSetting);
            }
        }
    }

    private LeagueSettingInfoSender HandleMappingForMultipleSettings(EventPickingModel match, List<LeagueSettingInfoSender> leagueSettings, List<Condition> tvList)
    {
        var leagueWithMatches = leagueSettings
            .Select(league => new
            {
                League = league,
                RoundDistance = league.FullConditionDescription is null
                    ? 0
                    : CheckMatchValidMappingWithLeagueManagement(match, league, tvList)
            })
            .Where(x => x.RoundDistance.HasValue)
            .ToList();

        if (leagueWithMatches.Count == 0)
        {
            return null;
        }

        return leagueWithMatches.Count > 1
            ? GetLeagueSettingByPriority(leagueWithMatches)
            : leagueWithMatches[0].League;
    }

    private LeagueSettingInfoSender GetLeagueSettingByPriority<T>(List<T> leagueSettings) where T : class
    {
        // Order by priority, round distance (lowest first), then by ID (descending)
        var orderedLeagues = leagueSettings
            .Select(x => new
            {
                League = (LeagueSettingInfoSender)x.GetType().GetProperty("League").GetValue(x),
                RoundDistance = (int?)x.GetType().GetProperty("RoundDistance").GetValue(x)
            })
            .OrderBy(x => GetPriority(x.League.Condition))
            .ThenBy(x => x.RoundDistance ?? int.MaxValue)
            .ThenByDescending(x => x.League.LeagueSettingId)
            .ToList();

        return orderedLeagues.FirstOrDefault()?.League;
    }

    #pragma warning disable S1541
     private int GetPriority(NewLeagueSettingCondition condition)
    {
        if (condition.HasTv && condition.Round.Count > 0 && condition.Division.Count > 0) return 1;
        if (condition.HasTv && condition.Round.Count > 0) return 2;
        if (condition.HasTv && condition.Division.Count > 0) return 3;
        if (condition.HasIpTv && condition.Round.Count > 0 && condition.Division.Count > 0) return 4;
        if (condition.HasIpTv && condition.Round.Count > 0) return 5;
        if (condition.HasIpTv && condition.Division.Count > 0) return 6;
        if (condition.HasTv) return 7;
        if (condition.HasIpTv) return 8;
        if (condition.Round.Count > 0 && condition.Division.Count > 0) return 9;
        if (condition.Round.Count > 0) return 10;
        if (condition.Division.Count > 0) return 11;
        return 12;
    }
    #pragma warning restore S1541

    private int? CheckMatchValidMappingWithLeagueManagement(EventPickingModel match, LeagueSettingInfoSender leagueSetting, List<Condition> tvList)
    {
        var roundDistance = CheckRoundConditions(match.Round, leagueSetting.Condition.Round);
        var divisionConditions = CheckDivisionConditions(match.Division, leagueSetting.Condition.Division);
        var tvConditions = CheckTVConditions(match, leagueSetting, tvList);

        if (roundDistance.HasValue && divisionConditions && tvConditions)
        {
            return roundDistance.Value;
        }

        return null;
    }

    private static int? CheckRoundConditions(string matchValues, IList<Condition> conditions)
    {
        var isEquals = CheckConditionEqual(matchValues, conditions);
        if (isEquals)
        {
            return 0;
        }
        if (!conditions.Any())
        {
            return int.MaxValue;
        }
        var conditionContains = GetConditionContain(matchValues, conditions);
        if (!conditionContains.Any())
        {
            return null;
        }

        var allConditionValues = conditionContains
            .Select(c => new
            {
                Value = c.Value,
                Distance = Levenshtein.Distance(matchValues.ToLowerInvariant(), c.Value.ToLowerInvariant())
            })
            .OrderBy(x => x.Distance)
            .ToList();

        // Get the best match (lowest distance)
        var bestMatch = allConditionValues[0];

        return bestMatch.Distance;
    }

    private static bool CheckDivisionConditions(string matchValues, IList<Condition> conditions)
    {
        var isContains = CheckConditionContain(matchValues, conditions);
        var isEquals = CheckConditionEqual(matchValues, conditions);
        var isPairWiseContains = CheckConditionPairWiseContain(matchValues, conditions);
        return !conditions.Any() || isContains || isEquals || isPairWiseContains;
    }

    private static bool CheckConditionPairWiseContain(string matchValues, IList<Condition> conditions)
    {
        var containsValues = GetValuePairWiseContains(conditions);
        var homeAwayDivision = matchValues.Split("-");
        if (homeAwayDivision.Length != 2)
        {
            return false;
        }

        var homeDivision = homeAwayDivision[0];
        var awayDivision = homeAwayDivision[1];

        return containsValues.Any(c =>
        {
            var homeAwayCondition = c.Split(",");
            if (homeAwayCondition.Length != 2)
            {
                return false;
            }
            var firstConditionValue = homeAwayCondition[0].Trim();
            var secondConditionValue = homeAwayCondition[1].Trim();

            return (homeDivision.Contains(firstConditionValue) &&
                awayDivision.Contains(secondConditionValue))
                ||
                (homeDivision.Contains(secondConditionValue) &&
                awayDivision.Contains(firstConditionValue));
        });
    }

    private static bool CheckConditionContain(string matchValues, IList<Condition> conditions)
    {
        var containsValues = GetValueContains(conditions);

        if (string.IsNullOrEmpty(matchValues))
        {
            return false;
        }

        return containsValues.Any(x => matchValues.Contains(x, StringComparison.CurrentCultureIgnoreCase)) && !string.IsNullOrEmpty(matchValues);
    }

    private static List<Condition> GetConditionContain(string matchValues, IList<Condition> conditions)
    {
        if (string.IsNullOrEmpty(matchValues))
        {
            return new List<Condition>();
        }

        return conditions
            .Where(c => c.Type == "Contains" &&
                        !string.IsNullOrEmpty(c.Value) &&
                        matchValues.Contains(c.Value, StringComparison.CurrentCultureIgnoreCase))
            .ToList();
    }

    private static bool CheckConditionEqual(string matchValues, IList<Condition> conditions)
    {
        var equalsValues = GetValueEquals(conditions);

        return equalsValues.Any(x => x.Equals(matchValues, StringComparison.CurrentCultureIgnoreCase));
    }

    private static bool CheckTVConditions(EventPickingModel match, LeagueSettingInfoSender leagueSetting, List<Condition> tvList)
    {
        var (hasTv, hasIpTv) = DetermineTvIptvStatus(match, tvList);
        var hasTvCondition = leagueSetting.Condition.HasTv;
        var hasIpTvCondition = leagueSetting.Condition.HasIpTv;

        if (hasTvCondition && hasIpTvCondition)
        {
            return hasTv || hasIpTv;
        }
        else if (hasTvCondition)
        {
            return hasTv;
        }
        else if (hasIpTvCondition)
        {
            return hasIpTv;
        }
        else
        {
            return true;
        }
    }

    private static IList<string> GetValuePairWiseContains(IList<Condition> conditions)
        => conditions.Where(x => x.Type == "Pairwise Contains").Select(x => x.Value).ToList();

    private static IList<string> GetValueContains(IList<Condition> conditions)
        => conditions.Where(x => x.Type == "Contains").Select(x => x.Value).ToList();

    private static IList<string> GetValueEquals(IList<Condition> conditions)
        => conditions.Where(x => x.Type == "Equals").Select(x => x.Value).ToList();

    private static (bool hasTv, bool hasIpTv) DetermineTvIptvStatus(EventPickingModel match, List<Condition> tvList)
    {
        var hasValidTv = !string.IsNullOrWhiteSpace(match.TV) &&
                        !match.TV.Contains("Cancel", StringComparison.OrdinalIgnoreCase) &&
                        tvList.Exists(tvCondition => IsTvConditionMatch(match.TV, tvCondition));

        var hasValidIptv = !string.IsNullOrWhiteSpace(match.Iptv) &&
                        !match.Iptv.Contains("Cancel", StringComparison.OrdinalIgnoreCase) &&
                        match.Iptv.Contains("IPTV", StringComparison.OrdinalIgnoreCase);

        var hasValidChannel = !string.IsNullOrWhiteSpace(match.Channel) &&
                            !match.Channel.Contains("Cancel", StringComparison.OrdinalIgnoreCase);
         return (hasValidTv, hasValidIptv || hasValidChannel);
    }

    private static bool IsTvConditionMatch(string tvField, Condition tvCondition)
    {
        if (string.IsNullOrWhiteSpace(tvField) || string.IsNullOrWhiteSpace(tvCondition.Value))
        {
            return false;
        }

        return tvCondition.Type switch
        {
            "Contains" => tvField.Contains(tvCondition.Value, StringComparison.OrdinalIgnoreCase),
            "Equals" => tvField.Equals(tvCondition.Value, StringComparison.OrdinalIgnoreCase),
            _ => false
        };
    }

    private void SuggestionValue(EventPickingModel match, LeagueSettingInfoSender league)
    {
        BuildNoOfferLive(match, league);
    }

    private void BuildNoOfferLive(EventPickingModel match, LeagueSettingInfoSender league)
    {
        match.NoOfferLive = league.NoOfferLive;
        match.IsNoOffer = ShouldSetNoOffer(league.NoOfferLive, match.EventDateTime);
    }

    private bool ShouldSetNoOffer(int noOfferLive, DateTime eventDateTime)
    {
        return noOfferLive switch
        {
            NoOfferConstant.WeekdayAndWeekend => true,
            NoOfferConstant.Weekend => IsWeekendMatch(eventDateTime),
            NoOfferConstant.WeekendPH => IsWeekendPeakHourMatch(eventDateTime),
            _ => false
        };
    }

    private bool IsWeekendMatch(DateTime matchTime)
    {
        var systemDate = matchTime.Hour < 12
            ? matchTime.Date.AddDays(-1)
            : matchTime.Date;

        return systemDate.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
    }

    private bool IsWeekendPeakHourMatch(DateTime matchTime)
    {
        if (!IsWeekendMatch(matchTime))
        {
            return false;
        }

        var time = matchTime.TimeOfDay;

        var isPeakHour =
            (time >= new TimeSpan(20, 0, 0))
            || (time <= new TimeSpan(2, 59, 0));

        return isPeakHour;
    }
}