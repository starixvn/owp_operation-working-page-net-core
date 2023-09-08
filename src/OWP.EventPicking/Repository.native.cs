using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using Newtonsoft.Json;
using OWP.EventPicking.Criteria;
using OWP.EventPicking.Handlers;
using OWP.EventPicking.Models;

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
}
