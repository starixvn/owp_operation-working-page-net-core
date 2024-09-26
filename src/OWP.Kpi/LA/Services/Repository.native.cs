using Fanex.Data.Repository;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Client.Services;
using Newtonsoft.Json;
using OWP.Kpi.LA.Criteria;
using OWP.Kpi.LA.Models;

namespace OWP.Kpi.LA.Services;

public interface Repository
{
    Task<IList<KpiInfo>> GetKpiInfo(QueryModel request);

    Task UpdateTargetLevel(List<TargetLevel> targetLevels);

    Task<IEnumerable<SectionInfo>> GetAllSection(int userId);
}

public class RepositoryImp : Repository
{
    private readonly IDynamicRepository repository;
    private readonly INapSession session;
    private readonly IUserService userService;

    private readonly INapSettings napSettings;

    public RepositoryImp(IDynamicRepository repository, INapSession session, INapSettings napSettings, IUserService userService)
    {
        this.repository = repository;
        this.session = session;
        this.napSettings = napSettings;
        this.userService = userService;
    }

    public async Task<IList<KpiInfo>> GetKpiInfo(QueryModel request)
    {
        var allUser = await userService.GetBySiteAsync(napSettings.SiteId, null);
        var allUserActive = await userService.GetBySiteAsync(napSettings.SiteId, true);

        return (await repository.FetchAsync<KpiInfo>(new CalculateKpiCriteria
        {
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            SectionIds = request.SectionIds,
            ActiveUser = request.ActiveUser,
            ResignUser = request.ResignUser,
            AllUserJson = JsonConvert.SerializeObject(allUser.Select(u => new
            {
                UserId = u.Id,
                IsActive = allUserActive.Any(a => a.Id == u.Id)
            }))
        })).ToList();
    }


    public async Task UpdateTargetLevel(List<TargetLevel> targetLevels)
        => await repository.FetchAsync<KpiInfo>(new UpdateTargetLevelCriteria
        {
            TargetJson = JsonConvert.SerializeObject(targetLevels),
            ActorId = session.UserId
        });

    public async Task<IEnumerable<SectionInfo>> GetAllSection(int userId)
        => await repository.FetchAsync<SectionInfo>(new GetAllSectionCriteria { UserId = userId });
}
