using Fanex.Data.Repository;
using OWP.Report.LAReport.Criteria;
using OWP.Report.LAReport.Models;
using OWP.Report.Models;

namespace OWP.Report.LAReport.Services;

public interface QueryRepository
{
    Task<IEnumerable<LAReportModel>> Query(QueryModel model);
}

public class QueryRepositoryImp : QueryRepository
{
    private readonly IDynamicRepository dynamicRepository;

    public QueryRepositoryImp(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public async Task<IEnumerable<LAReportModel>> Query(QueryModel model)
    {
        var criteria = new GetLAReportCriteria
        {
            FromDate = model.FromDate,
            ToDate = model.ToDate
        };

        return await dynamicRepository.FetchAsync<LAReportModel>(criteria);
    }
            
}