using Fanex.Data.Repository;
using Fanex.NAP.Client;
using OWP.Extensions;
using OWP.OSLeagueSetting.Criteria;
using OWP.OSLeagueSetting.Handlers;
using OWP.OSLeagueSetting.Models;

namespace OWP.OSLeagueSetting;

public interface Repository
{
    Task<IEnumerable<LeagueSettingInfo>> GetLeagueSettingInfo(GetLeagueInfoHandler.Request request);

    Task AddOrEditSetting(AddOrEditSettingCommonCriteria criteria);

    Task<(IEnumerable<UserLogModel>, IEnumerable<UserLogModel>)> GetLogSetting(int leagueSettingId);

    Task<IEnumerable<DuplicateSettingInfo>> GetLeagueSettingByLeagueName(int? leagueSettingId, string leagueName);
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

    public async Task AddOrEditSetting(AddOrEditSettingCommonCriteria criteria)
    {
        criteria.ThrowExceptionIfInvalid();
        criteria.UserId = currentSession.UserId;
        await dynamicRepository.ExecuteAsync(criteria);
    }

    public async Task<(IEnumerable<UserLogModel>, IEnumerable<UserLogModel>)> GetLogSetting(int leagueSettingId)
    {
        var criteria = new GetLogSettingCriteria { LeagueSettingId = leagueSettingId };
        return await dynamicRepository.FetchMultipleAsync<UserLogModel, UserLogModel>(criteria);
    }

    public async Task<IEnumerable<DuplicateSettingInfo>> GetLeagueSettingByLeagueName(int? leagueSettingId, string leagueName)
    {
        var criteria = new GetLeagueSettingByLeagueNameCriteria { LeagueSettingId = leagueSettingId, LeagueName = leagueName };
        return await dynamicRepository.FetchAsync<DuplicateSettingInfo>(criteria);
    }
}