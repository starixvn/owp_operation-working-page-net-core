namespace OWP.Report.LAReport.Services;

using Criteria;
using Fanex.Data.Repository;
using Models;
using Report.Models;

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