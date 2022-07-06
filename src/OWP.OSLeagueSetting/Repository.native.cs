using OWP.OSLeagueSetting.Criteria;

namespace OWP.OSLeagueSetting;

using Models;
using Fanex.Data.Repository;
using OWP.OSLeagueSetting.Handlers;

public interface Repository
{
    Task<IEnumerable<LeagueSettingInfo>> GetLeagueSettingInfo(GetLeagueInfoHandler.Request request);

    Task<SettingDetail> GetLeagueSettingDetail(int leagueSettingId);
}

public class RepositoryImp : Repository
{
    private readonly IDynamicRepository dynamicRepository;

    public RepositoryImp(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public async Task<IEnumerable<LeagueSettingInfo>> GetLeagueSettingInfo(GetLeagueInfoHandler.Request request)
    {
        var criteria = new GetLeagueSettingInfoCriteria
        {
            CategoryId = request.CategoryId,
            RoundId = request.RoundId,
            Status = request.StatusId,
            league = request.League
        };
        return await dynamicRepository.FetchAsync<LeagueSettingInfo>(criteria);
    }

    public async Task<SettingDetail> GetLeagueSettingDetail(int leagueSettingId)
    {
        var criteria = new QueryLeagueSettingDetailCriteria { LeagueSettingId = leagueSettingId };

        return (await dynamicRepository.FetchAsync<SettingDetail>(criteria)).First();
    }
}