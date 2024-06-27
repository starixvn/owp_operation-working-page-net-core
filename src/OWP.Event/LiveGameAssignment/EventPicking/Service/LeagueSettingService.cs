namespace OWP.Event.LiveGameAssignment.EventPicking.Service;

using AutoMapper;
using Criteria;
using Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

public interface LeagueSettingService
{
    Task<IList<MatchExporting>> ApplyMatchMaxbet(IList<MatchExporting> matches);
}

public class LeagueSettingServiceImp : LeagueSettingService
{
    private readonly LeagueSettingRepository repository;
    private readonly IMapper mapper;

    public LeagueSettingServiceImp(LeagueSettingRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<IList<MatchExporting>> ApplyMatchMaxbet(IList<MatchExporting> matches)
    {
        var leagueSettings = await GetLeagueSetting(new GetLeagueSettingCriteria());
        var tvList = await GetTVList(new GetTVListCriteria());
        var result = new List<MatchExporting>();

        foreach (var match in matches)
        {
            var key = match.LeagueName;
            var syncedMatch = match;

            if (leagueSettings.ContainsKey(key))
            {
                syncedMatch = SyncMaxbetToInfoSiteMatch(match, leagueSettings[key], tvList);
            }

            result.Add(syncedMatch);
        }

        return result;
    }

    private async Task<IEnumerable<Condition>> GetTVList(GetTVListCriteria criteria)
    {
        var tvList = await repository.GetTVList(criteria);
        return mapper.Map<IEnumerable<Condition>>(tvList);
    }

    private async Task<Dictionary<string, List<LeagueSetting>>> GetLeagueSetting(GetLeagueSettingCriteria criteria)
    {
        var leagueSettings = await repository.GetLeagueSettings(criteria);
        var leagueSettingsDic = leagueSettings.GroupBy(x => x.LeagueName.ToLowerInvariant())
            .Where(x => x.Any())
            .ToDictionary(x => x.Key, x => x.ToList());

        return new Dictionary<string, List<LeagueSetting>>(leagueSettingsDic, StringComparer.OrdinalIgnoreCase);
    }

    private static MatchExporting SyncMaxbetToInfoSiteMatch(MatchExporting match, IList<LeagueSetting> leagueSettings, IEnumerable<Condition> tvList)
    {
        var truthTables = new List<TruthTable>();
        var eventDateTime = DateTime.SpecifyKind(DateTime.Parse(match.game_date, CultureInfo.InvariantCulture), DateTimeKind.Utc).AddHours(-4);
        var isWK = eventDateTime.DayOfWeek == DayOfWeek.Saturday || eventDateTime.DayOfWeek == DayOfWeek.Sunday;

        foreach (var setting in leagueSettings)
        {
            var truthTable = GenerateTruthTable(match, setting, tvList);

            if (truthTable.Division != 0 && truthTable.Round != 0 && truthTable.TV != 0)
            {
                truthTables.Add(truthTable);
            }
        }

        var chosenTruthTable = truthTables.OrderByDescending(x => x.NumberOfTrue)
                                        .ThenByDescending(x => isWK ? x.MaxbetWK_HDP : x.MaxbetWD_HDP)
                                        .FirstOrDefault();

        if (chosenTruthTable == null)
        {
            return match;
        }

        var chosenLeagueSetting = leagueSettings.FirstOrDefault(x => x.LeagueSettingId == chosenTruthTable.LeagueSettingId);
        var result = MapSettings(match, chosenLeagueSetting, isWK);

        return result;
    }

    private static MatchExporting MapSettings(MatchExporting match, LeagueSetting leagueSetting, bool isWK)
    {
        return isWK
            ? MapWeekendSettings(match, leagueSetting)
            : MapWeekdaySettings(match, leagueSetting);
    }

    private static MatchExporting MapWeekendSettings(MatchExporting match, LeagueSetting leagueSetting)
    {
        match.Spread_HDP = leagueSetting.Spread_HDP;
        match.Spread_OU = leagueSetting.Spread_OU;

        match.MatchOffer = leagueSetting.MaxbetWK_MatchOffer;
        match.Maxbet_HDP = ConvertMaxbet(leagueSetting.MaxbetWK_HDP);
        match.Maxbet_OU = ConvertMaxbet(leagueSetting.MaxbetWK_OU);
        match.NoOfHDP = leagueSetting.MaxbetWK_NoOfHDP;

        match.LeagueRemark = leagueSetting.FormatedRemark;
        match.LSTNote = leagueSetting.FormatedLSTNote;

        return match;
    }

    private static MatchExporting MapWeekdaySettings(MatchExporting match, LeagueSetting leagueSetting)
    {
        match.Spread_HDP = leagueSetting.Spread_HDP;
        match.Spread_OU = leagueSetting.Spread_OU;

        match.MatchOffer = leagueSetting.MaxbetWD_MatchOffer;
        match.Maxbet_HDP = ConvertMaxbet(leagueSetting.MaxbetWD_HDP);
        match.Maxbet_OU = ConvertMaxbet(leagueSetting.MaxbetWD_OU);
        match.NoOfHDP = leagueSetting.MaxbetWD_NoOfHDP;

        match.LeagueRemark = leagueSetting.FormatedRemark;
        match.LSTNote = leagueSetting.FormatedLSTNote;

        return match;
    }

    private static string ConvertMaxbet(decimal? value)
    {
        if (value.HasValue)
        {
            return $"{(value.Value / 1000).ToString("0.###")} K";
        }

        return string.Empty;
    }

    private static TruthTable GenerateTruthTable(MatchExporting match, LeagueSetting leagueSettings, IEnumerable<Condition> tvList)
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

        result.Round = firstRule.Execute(leagueSettingCondition.Rounds, match.Round);
        result.Division = firstRule.Execute(leagueSettingCondition.Divisions, match.OutSide);

        result.NumberOfTrue += result.TV == 1 ? 1 : 0;
        result.NumberOfTrue += result.Division == 1 ? 1 : 0;
        result.NumberOfTrue += result.Round == 1 ? 1 : 0;

        return result;
    }
}