using Fanex.Data.Repository;
using OWP.Kpi.LA.Criteria;
using OWP.Kpi.LA.Models;

namespace OWP.Kpi.LA.Services;

public interface KpiService
{
    Task<IList<KpiInfo>> GetKpiInfo(DateTime fromDate, DateTime toDate);
}

public class KpiServiceImp : KpiService
{
    private readonly DynamicRepository repository;

    public KpiServiceImp(DynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IList<KpiInfo>> GetKpiInfo(DateTime fromDate, DateTime toDate)
    => (await repository.FetchAsync<KpiInfo>(new CalculateKpiCriteria
    {
        FromDate = fromDate,
        ToDate = toDate
    })).ToList();
}
