using Fanex.Data.Repository;

using Newtonsoft.Json;
using OWP.EventPicking.Api.Criteria;
using OWP.EventPicking.Api.Handlers;
using OWP.EventPicking.Api.Models;


namespace OWP.EventPicking;

public interface Repository
{
    Task<List<EventPickingModel>> QueryMainMatch(DateTime date);

    Task<List<EventPickingModel>> QueryChildMatch(DateTime date);

    Task<Dictionary<int, List<LeagueSetting>>> GetLeagueSettings();

    Task<IEnumerable<Condition>> GetTVList();

    Task InsertOrDeleteNoOfferMatch(InsertOrDeleteNoOfferMatchRequest request);

    Task<List<string>> GetProviderArrangement();

    Task<List<RAndDInfo>> GetRAndDUpdateInfo(DateTime date);

    Task<List<LogModel>> GetLog(int infoSiteMatchId);

    Task<List<Option>> GetUpdateType();

    Task UpdateConfirmed(UpdateConfirmedMatchCriteria updateConfirmedMatchCriteria);

    Task<IEnumerable<ExportLSTFormat>> QueryEventPickingESTFormat(DateTime date);

    Task<IEnumerable<ExportLSTFormat>> QueryChildMatchESTFormat(DateTime date);

    Task<IEnumerable<LeagueSettingInfoSender>> QueryLeagueSetting();

    Task UpdateEvent(UpdateMaxbetSpreadCriteria criteria);

    Task ExecuteNonQuery(INonQueryCommand criteria);

    Task<List<EventPickingModel>> QueryEventPickingChildMatch(DateTime date);

}

public class RepositoryImp : Repository
{
    private readonly IDynamicRepository dynamicRepository;

    private readonly INapSession session;

    public RepositoryImp(IDynamicRepository dynamicRepository, INapSession session)
    {
        this.dynamicRepository = dynamicRepository;
        this.session = session;
    }

    public async Task<List<EventPickingModel>> QueryMainMatch(DateTime date)
    {
        var criteria = new QueryMainMatchCriteria
        {
            Date = date
        };

        var result = await dynamicRepository.FetchAsync<EventPickingModel>(criteria);

        return result.ToList();
    }

    public async Task<List<EventPickingModel>> QueryChildMatch(DateTime date)
    {
        var criteria = new QueryChildMatchCriteria
        {
            Date = date
        };

        var result = await dynamicRepository.FetchAsync<EventPickingModel>(criteria);

        return result.ToList();
    }

    public async Task<Dictionary<int, List<LeagueSetting>>> GetLeagueSettings()
    {
        var leagueSettings = await dynamicRepository.FetchAsync<LeagueSetting>(new GetLeagueSettingCriteria());

        return leagueSettings
            .GroupBy(x => x.OWPLeagueId)
            .ToDictionary(x => x.Key, x => x.ToList());
    }

    public async Task<IEnumerable<Condition>> GetTVList()
    {
        var result = await dynamicRepository.FetchAsync<TVList>(new GetTVListCriteria());

        return result.Select(x => new Condition { Type = x.Label, Value = x.Value });
    }

    public async Task InsertOrDeleteNoOfferMatch(InsertOrDeleteNoOfferMatchRequest request)
    {
        var criteria = new InsertOrDeleteNoOfferMatchCriteria
        {
            MatchInfoJson = JsonConvert.SerializeObject(request.NoOfferMatch),
            ActorId = session.UserId
        };
        await dynamicRepository.ExecuteAsync(criteria);
    }

    public async Task<List<string>> GetProviderArrangement()
    {
        var criteria = new GetProviderCriteria();
        var provider = (await dynamicRepository.FetchAsync<ProviderInfo>(criteria))
            .OrderBy(x => x.Order)
            .Select(x => x.Value)
            .ToList();

        return provider;
    }

    public async Task<List<RAndDInfo>> GetRAndDUpdateInfo(DateTime date)
    {
        var criteria = new QueryRAndDInfoCriteria
        {
            Date = date
        };

        var result = await dynamicRepository.FetchAsync<RAndDInfo>(criteria);

        return result.ToList();
    }

    public async Task<List<Option>> GetUpdateType()
    {
        var criteria = new QueryUpdateTypeCriteria();

        var result = await dynamicRepository.FetchAsync<Option>(criteria);

        return result.ToList();
    }

    public async Task<List<LogModel>> GetLog(int infoSiteMatchId)
    => (await dynamicRepository.FetchAsync<LogModel>(new GetLogCriteria
    {
        InfoSiteMatchId = infoSiteMatchId
    })).ToList();

    public async Task UpdateConfirmed(UpdateConfirmedMatchCriteria criteria)
    => await dynamicRepository.ExecuteAsync(criteria);

    public async Task<IEnumerable<ExportLSTFormat>> QueryChildMatchESTFormat(DateTime date)
    {
        var criteria = new QueryChildMatchESTFormatCriteria { Date = date };
        var matches = await dynamicRepository.FetchAsync<ExportLSTFormat>(criteria);
        return matches;
    }

    public async Task<IEnumerable<ExportLSTFormat>> QueryEventPickingESTFormat(DateTime date)
    {
        var criteria = new QueryEventPickingESTFormatCriteria { Date = date };
        var matches = await dynamicRepository.FetchAsync<ExportLSTFormat>(criteria);
        return matches;
    }

    public async Task<IEnumerable<LeagueSettingInfoSender>> QueryLeagueSetting()
        => await dynamicRepository.FetchAsync<LeagueSettingInfoSender>(new GetNewLeagueSettingCriteria());

    public async Task UpdateEvent(UpdateMaxbetSpreadCriteria criteria)
        => await dynamicRepository.ExecuteAsync(criteria);

    public async Task ExecuteNonQuery(INonQueryCommand criteria)
    => await dynamicRepository.ExecuteAsync(criteria);

    public async Task<List<EventPickingModel>> QueryEventPickingChildMatch(DateTime date)
    {
        var criteria = new QueryEventPickingChildMatchCriteria
        {
            Date = date
        };

        var result = await dynamicRepository.FetchAsync<EventPickingModel>(criteria);

        return result.ToList();
    }

}
