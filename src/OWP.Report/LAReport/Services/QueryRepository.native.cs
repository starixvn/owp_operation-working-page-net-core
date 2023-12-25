using Fanex.Data.Repository;
using Fanex.NAP.Client.Configuration;
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

    private readonly INapSettings napSettings;

    public QueryRepositoryImp(IDynamicRepository dynamicRepository, INapSettings napSettings)
    {
        this.dynamicRepository = dynamicRepository;
        this.napSettings = napSettings;
    }

    public async Task<IEnumerable<LAReportModel>> Query(QueryModel model)
    {
        var criteria = new GetLAReportCriteria
        {
            FromDate = model.FromDate,
            ToDate = model.ToDate,
            SiteId = napSettings.SiteId,
            ActiveUser = model.ActiveUser,
            ResignUser = model.ResignUser
        };

        return await dynamicRepository.FetchAsync<LAReportModel>(criteria);
    }

}