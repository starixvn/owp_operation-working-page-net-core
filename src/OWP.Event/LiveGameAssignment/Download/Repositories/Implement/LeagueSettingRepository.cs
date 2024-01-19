namespace OWP.Event.LiveGameAssignment.Download.Repositories.Implement;

using System.Collections.Generic;
using System.Threading.Tasks;
using Criteria.EventPicking;
using Fanex.Data.Repository;
using Models.EventPicking;

public interface LeagueSettingRepository
{
    Task<IEnumerable<BlackList>> GetBlackListByLeagueName(GetBlackListCriteria criteria);

    Task<IEnumerable<TVList>> GetTVList(GetTVListCriteria criteria);

    Task<IEnumerable<LeagueSetting>> GetLeagueSettings(GetLeagueSettingCriteria criteria);
}

public class LeagueSettingRepositoryImp : LeagueSettingRepository
{
    private readonly IDynamicRepository dynamicRepository;

    public LeagueSettingRepositoryImp(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public async Task<IEnumerable<BlackList>> GetBlackListByLeagueName(GetBlackListCriteria criteria)
    => await dynamicRepository.FetchAsync<BlackList>(criteria);

    public async Task<IEnumerable<LeagueSetting>> GetLeagueSettings(GetLeagueSettingCriteria criteria) => await dynamicRepository.FetchAsync<LeagueSetting>(criteria);

    public async Task<IEnumerable<TVList>> GetTVList(GetTVListCriteria criteria) => await dynamicRepository.FetchAsync<TVList>(criteria);
}