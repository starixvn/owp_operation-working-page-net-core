using Fanex.Data.Repository;
using OWP.Report.Models;
using OWP.Report.TraderReport.Criteria;
using OWP.Report.TraderReport.Models;

namespace OWP.Report.TraderReport.Services;

public interface RepositoryService
{
    Task<IEnumerable<ReportModel>> GetLstReportDataByDate(QueryModel queryModel);
}

public class RepositoryServiceImp : RepositoryService
{
    private readonly IDynamicRepository dynamicRepository;

    public RepositoryServiceImp(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public async Task<IEnumerable<ReportModel>> GetLstReportDataByDate(QueryModel queryModel)
    {
        var criteria = new GetTraderReportCriteria
        {
            FromDate = queryModel.FromDate,
            ToDate = queryModel.ToDate
        };

        return await dynamicRepository.FetchAsync<ReportModel>(criteria);
    }
}