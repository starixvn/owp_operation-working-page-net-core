using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using Newtonsoft.Json;
using OWP.Kpi.LA.Criteria;
using OWP.Kpi.LA.Models;

namespace OWP.Kpi.LA.Services;

public interface Repository
{
    Task<IList<KpiInfo>> GetKpiInfo(DateTime fromDate, DateTime toDate, string sectionIds);

    Task UpdateTargetLevel(List<TargetLevel> targetLevels);

    Task<IEnumerable<SectionInfo>> GetAllSection( int userId);
}

public class RepositoryImp : Repository
{
    private readonly IDynamicRepository repository;
    private readonly INapSession session;

    public RepositoryImp(IDynamicRepository repository, INapSession session)
    {
        this.repository = repository;
        this.session = session;
    }

    public async Task<IList<KpiInfo>> GetKpiInfo(DateTime fromDate, DateTime toDate, string sectionIds)
        => (await repository.FetchAsync<KpiInfo>(new CalculateKpiCriteria
        {
            FromDate = fromDate,
            ToDate = toDate,
            SectionIds = sectionIds
        })).ToList();

    public async Task UpdateTargetLevel(List<TargetLevel> targetLevels)
        => await repository.FetchAsync<KpiInfo>(new UpdateTargetLevelCriteria
        {
            TargetJson = JsonConvert.SerializeObject(targetLevels),
            ActorId = session.UserId
        });

    public async Task<IEnumerable<SectionInfo>> GetAllSection(int userId)
        => await repository.FetchAsync<SectionInfo>(new GetAllSectionCriteria { UserId = userId });
}
