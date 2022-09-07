namespace OWP.Maxbet;

using Criteria;
using Fanex.Data.Repository;
using Models;

public interface MaxbetRepository
{
    public Task<int> GetMatchIdByAdminSiteId(int adminSiteId);

    public Task<MinorMatchInfo> GetMatchIdByAdminSiteId(int adminSiteId, int bettypeId);

    public Task<MatchInfo> GetMatchInfoByAdminSiteId(int adminSiteId);

    public Task<List<TraderAssignment>> GetTradersByOwpMatchId(int owpMatchId);

    public Task<ValueTuple<IEnumerable<SpreadModel>, IEnumerable<LeagueGroup>>> GetSpreadByOwpMatchId(int owpMatchId);

    public Task<ValueTuple<IEnumerable<SpreadModel>, IEnumerable<LeagueGroup>>> GetSpreadSpecialProductById(int owpMatchId);

    public Task<IEnumerable<MaxbetModel>> GetMaxbetByOwpMatchId(int matchId);

    public Task<IEnumerable<int?>> GetTraderXMins(int specialProductId);

    public Task<IEnumerable<decimal?>> GetSpreadXMins(int owpMatchId);

    public Task<IEnumerable<decimal?>> GetMaxbetXMins(int owpMatchId);

        public Task<IEnumerable<Models.KeyValuePair>> GetOddByOwpId(int owpMatchId);
        
         public Task<IEnumerable<MaxbetSetting>> GetMaxbetSettingDefault();


    public Task<(SecondAcceptDefault, string)> GetSecondAcceptValueDefaultInfo(decimal? maxbetHDP, int OWPMatchId);

    public Task<IEnumerable<MatchResource>> GetMatchOfferId(int matchId);
}

public class MaxbetRepositoryImp : MaxbetRepository
{
    private readonly IDynamicRepository dynamicRepository;

    public MaxbetRepositoryImp(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public async Task<ValueTuple<IEnumerable<SpreadModel>, IEnumerable<LeagueGroup>>> GetSpreadByOwpMatchId(int owpMatchId)
    {
        var criteria = new SpreadGetCriteria
        {
            OwpMatchId = owpMatchId
        };
        var (firstResultSet, secondResultSet) = await dynamicRepository.FetchMultipleAsync<SpreadModel, LeagueGroup>(criteria);
        var valueTupple = new ValueTuple<IEnumerable<SpreadModel>, IEnumerable<LeagueGroup>>
        {
            Item1 = firstResultSet,
            Item2 = secondResultSet
        };
        return valueTupple;
    }

    public async Task<List<TraderAssignment>> GetTradersByOwpMatchId(int owpMatchId)
    {
        var criteria = new TradersGetCriteria
        {
            OwpMatchId = owpMatchId
        };
        return (await dynamicRepository.FetchAsync<TraderAssignment>(criteria)).ToList();
    }

    public async Task<int> GetMatchIdByAdminSiteId(int adminSiteId)
    {
        var criteria = new MatchIdGetCriteria
        {
            AdminsiteId = adminSiteId
        };

        var matchId = (await dynamicRepository.FetchAsync<int>(criteria)).FirstOrDefault();
        return matchId;
    }

    public async Task<IEnumerable<MaxbetModel>> GetMaxbetByOwpMatchId(int matchId)
        => await dynamicRepository.FetchAsync<MaxbetModel>(new MaxbetGetCriteria
        {
            OWPMatchId = matchId
        });

    public async Task<MinorMatchInfo> GetMatchIdByAdminSiteId(int adminSiteId, int bettypeId)
    {
        var criteria = new SpecialProductIdCriteria
        {
            AdminsiteId = adminSiteId,
            BettypeId = bettypeId
        };

        var matchId = (await dynamicRepository.FetchAsync<MinorMatchInfo>(criteria)).FirstOrDefault();
        return matchId;
    }

    public async Task<IEnumerable<int?>> GetTraderXMins(int specialProductId)
        => await dynamicRepository.FetchAsync<int?>(new SpecialProductTraderXMinsGetCriteria
        {
            OwpMatchId = specialProductId
        });

    public async Task<IEnumerable<decimal?>> GetSpreadXMins(int owpMatchId)
        => await dynamicRepository.FetchAsync<decimal?>(new SpecialProductSpreadXMinsGetCriteria
        {
            OwpMatchId = owpMatchId
        });

    public async Task<IEnumerable<decimal?>> GetMaxbetXMins(int owpMatchId)
        => await dynamicRepository.FetchAsync<decimal?>(new SpecialProductMaxbetXMinsGetCriteria
        {
            OwpMatchId = owpMatchId
        });

    public async Task<(IEnumerable<SpreadModel>, IEnumerable<LeagueGroup>)> GetSpreadSpecialProductById(int owpMatchId)
    {
        var criteria = new SpecialProductSpreadGetCriteria
        {
            OwpMatchId = owpMatchId
        };
        var (firstResultSet, secondResultSet) = await dynamicRepository.FetchMultipleAsync<SpreadModel, LeagueGroup>(criteria);
        var valueTupple = new ValueTuple<IEnumerable<SpreadModel>, IEnumerable<LeagueGroup>>
        {
            Item1 = firstResultSet,
            Item2 = secondResultSet
        };
        return valueTupple;
    }

    public async Task<MatchInfo> GetMatchInfoByAdminSiteId(int adminSiteId)
    {
        var criteria = new MatchInfoGetCriteria
        {
            AdminsiteId = adminSiteId
        };
        return (await dynamicRepository.FetchAsync<MatchInfo>(criteria)).FirstOrDefault();
        }

        public async Task<IEnumerable<Models.KeyValuePair>> GetOddByOwpId(int owpMatchId)
        {
            return await dynamicRepository.FetchAsync<Models.KeyValuePair>(new OddGetCriteria
            {
                OwpMatchId = owpMatchId
            });
    }

    public async Task<IEnumerable<MaxbetSetting>> GetMaxbetSettingDefault()
    {
        var criteria = new GetMaxbetSettingCriteria();

        return await dynamicRepository.FetchAsync<MaxbetSetting>(criteria);
    }

    public async Task<(SecondAcceptDefault, string)> GetSecondAcceptValueDefaultInfo(decimal? maxbetHDP, int OWPMatchId)
    {
        var criteria = new GetSecondAcceptValueDefaultCriteria { MaxbetHDP = maxbetHDP, OWPMatchId = OWPMatchId };
        var (secondAcceptDefaults, leagueNames)
            = await dynamicRepository.FetchMultipleAsync<SecondAcceptDefault, string>(criteria);

        return (secondAcceptDefaults.FirstOrDefault(), leagueNames.FirstOrDefault());
    }

    public async Task<IEnumerable<MatchResource>> GetMatchOfferId(int matchId)
    {
        var criteria = new MatchOfferGetCriteria
        {
            OWPMatchId = matchId
        };

        return await dynamicRepository.FetchAsync<MatchResource>(criteria);
    }
}