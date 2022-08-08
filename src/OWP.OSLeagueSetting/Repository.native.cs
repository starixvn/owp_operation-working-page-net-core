
namespace OWP.OSLeagueSetting;

using OWP.OSLeagueSetting.Criteria;
using Models;
using Fanex.Data.Repository;
using OWP.OSLeagueSetting.Handlers;
using OWP.Extensions;
using Fanex.NAP.Client.Helpers.Web;

public interface Repository
{
    Task<IEnumerable<LeagueSettingInfo>> GetLeagueSettingInfo(GetLeagueInfoHandler.Request request);

    Task<SettingDetail> GetLeagueSettingDetail(int leagueSettingId);

    Task AddOrEditSetting(AddOrEditSettingCommonCriteria criteria);
}

public class RepositoryImp : Repository
{
    private readonly INapSession currentSession;

    private readonly IDynamicRepository dynamicRepository;

    public RepositoryImp(IDynamicRepository dynamicRepository, INapSession currentSession)
    {
        this.dynamicRepository = dynamicRepository;
        this.currentSession = currentSession;
    }

    public async Task<IEnumerable<LeagueSettingInfo>> GetLeagueSettingInfo(GetLeagueInfoHandler.Request request)
    {
        var criteria = new GetLeagueSettingInfoCriteria
        {
            CategoryId = request.CategoryId,
            RoundId = request.RoundId,
            Status = request.StatusId,
            League = request.League
        };
        return await dynamicRepository.FetchAsync<LeagueSettingInfo>(criteria);
    }

    public async Task<SettingDetail> GetLeagueSettingDetail(int leagueSettingId)
    {
        var criteria = new QueryLeagueSettingDetailCriteria { LeagueSettingId = leagueSettingId };
        var result = (await dynamicRepository.FetchAsync<SettingDetail>(criteria));

        return result.Count() > 0 ? result.FirstOrDefault() : new SettingDetail();
    }

    public async Task AddOrEditSetting(AddOrEditSettingCommonCriteria criteria)
    {
        criteria.ThrowExceptionIfInvalid();
        criteria.UserId = currentSession.UserId;
        await dynamicRepository.ExecuteAsync(criteria);
    }
}