using Fanex.Data.Repository;
using Newtonsoft.Json;
using OWP.Kpi.LA.Criteria;
using OWP.Kpi.LA.Models;

namespace OWP.Kpi.LA.Services;

public interface Repository
{
    Task<IList<KpiInfo>> GetKpiInfo(DateTime fromDate, DateTime toDate);

    Task UpdateTargetLevel(List<TargetLevel> targetLevels);
}

public class RepositoryImp : Repository
{
    private readonly IDynamicRepository repository;

    public RepositoryImp(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IList<KpiInfo>> GetKpiInfo(DateTime fromDate, DateTime toDate)
        => (await repository.FetchAsync<KpiInfo>(new CalculateKpiCriteria
        {
            FromDate = fromDate,
            ToDate = toDate
        })).ToList();

    public async Task UpdateTargetLevel(List<TargetLevel> targetLevels)
        => await repository.FetchAsync<KpiInfo>(new UpdateTargetLevelCriteria
        {
            TargetJson = JsonConvert.SerializeObject(targetLevels)
        });
}
