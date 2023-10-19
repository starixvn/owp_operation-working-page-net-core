using OWP.EventPicking.Misc;
using OWP.EventPicking.Models;
using OWP.Extensions;
// ReSharper disable ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator

namespace OWP.EventPicking.Services;

public interface BuildMainMatchMaxBet
{
    Task BuildMainMatchMaxbet(IEnumerable<EventPickingModel> matches);
}

public class BuildMainMatchMaxBetImp : BuildMainMatchMaxBet
{
    private readonly Repository repository;

    public BuildMainMatchMaxBetImp(Repository repository)
    {
        this.repository = repository;
    }

    public async Task BuildMainMatchMaxbet(IEnumerable<EventPickingModel> matches)
    {
        var leagueSettings = await repository.GetLeagueSettings();
        var tvList = (await repository.GetTVList()).ToList();

        foreach (var match in matches)
        {
            var leagueSettingsById = leagueSettings.GetValue(match.OWPLeagueId, new List<LeagueSetting>());
            if (leagueSettingsById.Count > 0)
            {
                SyncMaxbetToInfoSiteMatch(match, leagueSettingsById, tvList);
            }
        }
    }

    private static void SyncMaxbetToInfoSiteMatch(EventPickingModel match, List<LeagueSetting> leagueSettings, List<Condition> tvList)
    {
        var truthTables = new List<TruthTable>();
        var isWeekend = IsWeekend(match.EventDateTime);

        foreach (var setting in leagueSettings)
        {
            var truthTable = GenerateTruthTable(match, setting, tvList);
            if (truthTable.Division != 0 && truthTable.Round != 0 && truthTable.TV != 0)
            {
                truthTables.Add(truthTable);
            }
        }

        var chosenTruthTable = truthTables.OrderByDescending(x => x.NumberOfTrue)
                                        .ThenByDescending(x => isWeekend ? x.MaxbetWK_HDP : x.MaxbetWD_HDP)
                                        .FirstOrDefault();

        if (chosenTruthTable == null)
        {
            return;
        }

        var chosenLeagueSetting =
            leagueSettings.Find(x => x.LeagueSettingId == chosenTruthTable.LeagueSettingId);
        MapSettings(match, chosenLeagueSetting, isWeekend);
    }

    private static bool IsWeekend(DateTime eventDateTime)
    {
        var eventDate = DateTime.SpecifyKind(eventDateTime, DateTimeKind.Utc).AddHours(-4);
        var isWeekend = eventDate.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
        return isWeekend;
    }

    private static void MapSettings(EventPickingModel match, LeagueSetting leagueSetting, bool isWK)
    {
        if (isWK)
        {
            match.MapWeekendSettings(leagueSetting);
        }
        else
        {
            match.MapWeekdaySettings(leagueSetting);
        }
    }

    private static TruthTable GenerateTruthTable(EventPickingModel match, LeagueSetting leagueSettings, List<Condition> tvList)
    {
        var result = new TruthTable
        {
            LeagueSettingId = leagueSettings.LeagueSettingId,
            MaxbetWD_HDP = leagueSettings.MaxbetWD_HDP,
            MaxbetWK_HDP = leagueSettings.MaxbetWK_HDP
        };

        var firstRule = new MatchContainsAnyLeagueItemRule();
        var secondRule = new BothAreEmptyRule();
        var thirdRule = new LeagueHasValueAndMatchIsEmptyRule();

        firstRule.SetSuccessor(secondRule);
        secondRule.SetSuccessor(thirdRule);

        int liveTVResult;
        int localTVResult;

        if (leagueSettings.HasTV)
        {
            liveTVResult = firstRule.Execute(tvList, match.LiveTV);
            localTVResult = firstRule.Execute(tvList, match.LocalTV);
        }
        else
        {
            var emptyTVList = new List<Condition>();
            liveTVResult = firstRule.Execute(emptyTVList, match.LiveTV);
            localTVResult = firstRule.Execute(emptyTVList, match.LocalTV);
        }


        result.TV = liveTVResult == 1 || localTVResult == 1 ? 1 : 0;

        var leagueSettingCondition = leagueSettings.GetFullConditionDes();

        result.Round = firstRule.Execute(leagueSettingCondition.Rounds.ToList(), match.Round);
        result.Division = firstRule.Execute(leagueSettingCondition.Divisions.ToList(), match.OutSide);

        result.NumberOfTrue += result.TV == 1 ? 1 : 0;
        result.NumberOfTrue += result.Division == 1 ? 1 : 0;
        result.NumberOfTrue += result.Round == 1 ? 1 : 0;

        return result;
    }
}