namespace OWP.Event.LiveGameAssignment.EventPicking.Service;

using AutoMapper;
using Criteria;
using Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Utils;

public interface LeagueSettingNewFormatService
{
    Task ApplyIsBlackList(IList<MatchExportingNewFormat> matches);

    Task ApplyMatchMaxbet(IList<MatchExportingNewFormat> matches);
}

public class LeagueSettingNewFormatServiceImp : LeagueSettingNewFormatService
{
    private readonly LeagueSettingRepository repository;
    private readonly IMapper mapper;

    public LeagueSettingNewFormatServiceImp(LeagueSettingRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task ApplyMatchMaxbet(IList<MatchExportingNewFormat> matches)
    {
        var leagueSettings = await GetLeagueSetting(new GetLeagueSettingCriteria());
        var tvList = await GetTVList(new GetTVListCriteria());

        foreach (var match in matches)
        {
            var key = match.LeagueName;

            if (leagueSettings.ContainsKey(key))
            {
                SyncMaxbetToInfoSiteMatch(match, leagueSettings[key], tvList);
            }
        }
    }

    private async Task<IEnumerable<Condition>> GetTVList(GetTVListCriteria criteria)
    {
        var tvList = await repository.GetTVList(criteria);
        return mapper.Map<IEnumerable<Condition>>(tvList);
    }

    private async Task<Dictionary<string, List<LeagueSetting>>> GetLeagueSetting(GetLeagueSettingCriteria criteria)
    {
        var leagueSettings = await repository.GetLeagueSettings(criteria);
        var leagueSettingsDic = leagueSettings.GroupBy(x => x.LeagueName, StringComparer.OrdinalIgnoreCase)
            .Where(x => x.Any())
            .ToDictionary(x => x.Key, x => x.ToList(), StringComparer.OrdinalIgnoreCase);

        return new Dictionary<string, List<LeagueSetting>>(leagueSettingsDic, StringComparer.OrdinalIgnoreCase);
    }

    private static void SyncMaxbetToInfoSiteMatch(MatchExportingNewFormat match, IList<LeagueSetting> leagueSettings, IEnumerable<Condition> tvList)
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

        if (chosenTruthTable != null)
        {
            var chosenLeagueSetting = leagueSettings.FirstOrDefault(x => x.LeagueSettingId == chosenTruthTable.LeagueSettingId);
            MapSettings(match, chosenLeagueSetting, isWK);
        }
    }

    private static void MapSettings(MatchExportingNewFormat match, LeagueSetting leagueSetting, bool isWK)
    {
        if (isWK)
        {
            MapWeekendSettings(match, leagueSetting);
        }
        else
        {
            MapWeekdaySettings(match, leagueSetting);
        }

    }

    private static void MapWeekendSettings(MatchExportingNewFormat match, LeagueSetting leagueSetting)
    {
        match.MatchOffer = leagueSetting.MaxbetWK_MatchOffer;
        match.LstNote = leagueSetting.FormatedLSTNote;
        match.MaxBetHDP = leagueSetting.MaxbetWK_HDP.ToOWPMaxBetFormat();
        match.MaxBetHDPNumber = leagueSetting.MaxbetWK_HDP;
        match.SpreadHDP = leagueSetting.Spread_HDP.ToOWPSpreadFormatPercent();
        match.SpreadOU = leagueSetting.Spread_OU.ToOWPSpreadFormatPercent();
        match.NoHDP = leagueSetting.MaxbetWK_NoOfHDP;

        match.MaxBetOU = leagueSetting.MaxbetWK_OU.ToOWPMaxBetFormat();
        match.MaxBet1HHDP = leagueSetting.MaxbetWK_1HHDP.ToOWPMaxBetFormat();
        match.MaxBet1HOU = leagueSetting.MaxbetWK_1HOU.ToOWPMaxBetFormat();
        match.Spread1HHDP = leagueSetting.Spread_1HHDP.ToOWPSpreadFormatPercent();
        match.Spread1HOU = leagueSetting.Spread_1HOU.ToOWPSpreadFormatPercent();
        match.MaxBetFT1X2 = leagueSetting.MaxBet_FT1X2.ToOWPMaxBetFormat();
        match.Spread1X2 = leagueSetting.MaxBet_1X2Spread.ToOWPSpreadFormatPercent();
        match.MaxBet1H1X2 = leagueSetting.MaxBet_1H1X2.ToOWPMaxBetFormat();
        match.Spread1H1X2 = leagueSetting.Spread_1H1X2.ToOWPSpreadFormatPercent();

        match.IsBlackList = leagueSetting.IsBlackList;
    }

    private static void MapWeekdaySettings(MatchExportingNewFormat match, LeagueSetting leagueSetting)
    {
        match.MatchOffer = leagueSetting.MaxbetWD_MatchOffer;
        match.LstNote = leagueSetting.FormatedLSTNote;
        match.MaxBetHDP = leagueSetting.MaxbetWD_HDP.ToOWPMaxBetFormat();
        match.MaxBetHDPNumber = leagueSetting.MaxbetWD_HDP;
        match.SpreadHDP = leagueSetting.Spread_HDP.ToOWPSpreadFormatPercent();
        match.SpreadOU = leagueSetting.Spread_OU.ToOWPSpreadFormatPercent();
        match.NoHDP = leagueSetting.MaxbetWD_NoOfHDP;

        match.MaxBetOU = leagueSetting.MaxbetWD_OU.ToOWPMaxBetFormat();
        match.MaxBet1HHDP = leagueSetting.MaxbetWD_1HHDP.ToOWPMaxBetFormat();
        match.MaxBet1HOU = leagueSetting.MaxbetWD_1HOU.ToOWPMaxBetFormat();
        match.Spread1HHDP = leagueSetting.Spread_1HHDP.ToOWPSpreadFormatPercent();
        match.Spread1HOU = leagueSetting.Spread_1HOU.ToOWPSpreadFormatPercent();
        match.MaxBetFT1X2 = leagueSetting.MaxBet_FT1X2.ToOWPMaxBetFormat();
        match.Spread1X2 = leagueSetting.MaxBet_1X2Spread.ToOWPSpreadFormatPercent();
        match.MaxBet1H1X2 = leagueSetting.MaxBet_1H1X2.ToOWPMaxBetFormat();
        match.Spread1H1X2 = leagueSetting.Spread_1H1X2.ToOWPSpreadFormatPercent();

        match.IsBlackList = leagueSetting.IsBlackList;
    }

    private static TruthTable GenerateTruthTable(MatchExportingNewFormat match, LeagueSetting leagueSettings, IEnumerable<Condition> tvList)
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

    public async Task ApplyIsBlackList(IList<MatchExportingNewFormat> matches)
    {
        var leagueNames = matches.Select(m => m.LeagueName);
        var criteria = new GetBlackListCriteria
        {
            LeagueNames = string.Join(",", leagueNames)
        };
        var blackLists = await repository.GetBlackListByLeagueName(criteria);
        foreach (var match in matches)
        {
            var blacklist = blackLists?.FirstOrDefault(b => string.Equals(b.LeagueName, match.LeagueName));
            if (blacklist != null)
            {
                match.IsBlackList = blacklist.IsBlackList;
            }
        }
    }
}