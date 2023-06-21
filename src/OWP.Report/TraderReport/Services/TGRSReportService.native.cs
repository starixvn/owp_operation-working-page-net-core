using Fanex.Logging;
using Microsoft.Extensions.Configuration;
using OWP.Constants;
using OWP.Report.Models;
using OWP.Report.TraderReport.Criteria;
using OWP.Report.TraderReport.Models;
using OWP.Services;

namespace OWP.Report.TraderReport.Services;

public interface TGRSReportService
{
    Task<IEnumerable<TraderPerformanceDetail>> GetTraderPerformanceDetails(List<int> traderIds, QueryModel queryModel);
}

public class TGRSReportServiceImp : TGRSReportService
{
    private readonly IConfiguration configuration;

    private readonly RichHttpClient richHttpClient;

    public TGRSReportServiceImp(RichHttpClient richHttpClient, IConfiguration configuration)
    {
        this.configuration = configuration;
        this.richHttpClient = richHttpClient;
    }

    public async Task<IEnumerable<TraderPerformanceDetail>> GetTraderPerformanceDetails(List<int> traderIds, QueryModel queryModel)
    {
        var majorBettypeIds = new[] { Bettype.Handicap, Bettype.OverUnder, Bettype.FirstHalfHandicap, Bettype.FirstHalfOverUnder };
        var criteria = new GetPerformanceDetailsCriteria
        {
            FromDate = queryModel.FromDate,
            ToDate = queryModel.ToDate,
            TraderIds = string.Join(",", traderIds),
            MajorBettypeIds = string.Join(",", majorBettypeIds)
        };

        try
        {
            var requestUrl = $"{configuration.GetSection("TRGS_Report_ApiEndpoint").Value}/Trader/GetPerformanceDetails";
            return await richHttpClient.Post<List<TraderPerformanceDetail>>(requestUrl, criteria);
        }
        catch (Exception exception)
        {
            Logger.Log.Error(exception.Message);
            return new List<TraderPerformanceDetail>();
        }
    }
}