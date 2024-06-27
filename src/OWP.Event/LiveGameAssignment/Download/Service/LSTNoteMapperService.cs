namespace OWP.Event.LiveGameAssignment.Download.Service;

using AutoMapper;
using EventPicking;
using EventPicking.Model;
using Models;
using OWP.Event.LiveGameAssignment.EventPicking.Criteria;
using OWP.Event.LiveGameAssignment.EventPicking.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface LstNoteMapperService
{
    Task<IList<EventModel>> ApplyLstTNote(IList<EventModel> matches);
}

public class LstNoteMapperServiceImp : LstNoteMapperService
{
    private readonly LeagueSettingRepository repository;
    private readonly IMapper mapper;

    public LstNoteMapperServiceImp(LeagueSettingRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<IList<EventModel>> ApplyLstTNote(IList<EventModel> matches)
    {
        var leagueSettings = await GetLeagueSetting(new GetLeagueSettingCriteria());
        var tvList = await GetTvList(new GetTVListCriteria());
        var result = new List<EventModel>();

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

        return result.ToList();
    }

    private async Task<Dictionary<string, List<LeagueSetting>>> GetLeagueSetting(GetLeagueSettingCriteria criteria)
    {
        var leagueSettings = await repository.GetLeagueSettings(criteria);
        var leagueSettingsDic = leagueSettings.GroupBy(x => x.LeagueName.ToLowerInvariant())
            .Where(x => x.Any())
            .ToDictionary(x => x.Key, x => x.ToList());

        return new Dictionary<string, List<LeagueSetting>>(leagueSettingsDic, StringComparer.OrdinalIgnoreCase);
    }

    private async Task<IEnumerable<Condition>> GetTvList(GetTVListCriteria criteria)
    {
        var tvList = await repository.GetTVList(criteria);
        return mapper.Map<IEnumerable<Condition>>(tvList);
    }

    private static EventModel SyncMaxbetToInfoSiteMatch(EventModel match, IList<LeagueSetting> leagueSettings, IEnumerable<Condition> tvList)
    {
        var truthTables = new List<TruthTable>();

        foreach (var setting in leagueSettings)
        {
            var truthTable = GenerateTruthTable(match, setting, tvList);

            if (truthTable.Division != 0 && truthTable.Round != 0 && truthTable.TV != 0)
            {
                truthTables.Add(truthTable);
            }
        }

        var chosenTruthTable = truthTables.OrderByDescending(x => x.NumberOfTrue).FirstOrDefault();

        if (chosenTruthTable == null)
        {
            return match;
        }

        var chosenLeagueSetting = leagueSettings.FirstOrDefault(x => x.LeagueSettingId == chosenTruthTable.LeagueSettingId);
        var result = MapLstNote(match, chosenLeagueSetting);

        return result;
    }

    private static TruthTable GenerateTruthTable(EventModel match, LeagueSetting leagueSettings, IEnumerable<Condition> tvList)
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
            liveTVResult = firstRule.Execute(tvList, match.Channel);
            localTVResult = firstRule.Execute(tvList, match.TVCode);
        }
        else
        {
            var emptyTVList = new List<Condition>();
            liveTVResult = firstRule.Execute(emptyTVList, match.Channel);
            localTVResult = firstRule.Execute(emptyTVList, match.TVCode);
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

    private static EventModel MapLstNote(EventModel match, LeagueSetting leagueSetting)
    {
        match.LSTNote = leagueSetting.FormatedLSTNote;
        return match;
    }
}
