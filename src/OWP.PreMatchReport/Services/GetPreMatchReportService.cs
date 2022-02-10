using OWP.PreMatchReport.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OWP.PreMatchReport.Models;
using OWP.PreMatchReport.Handlers;
using MediatR;

namespace OWP.PreMatchReport.Services;

public interface GetPreMatchReportService
{
    Task<IList<ReportModel>> GetReports(ReportRequest request);
}
public class GetPreMatchReportServiceImp : GetPreMatchReportService
{
    private readonly Repository repository;

    public GetPreMatchReportServiceImp(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<IList<ReportModel>> GetReports(ReportRequest request)
    {
        var queryReportLeague = await repository.GetReportForLeague(request);
        var queryReportMatches = await repository.GetReportForMatches(request);

        var groupedTraderLeague = queryReportLeague.GroupBy(x => x.TraderName).Select(y => y.ToList()).ToList();
        var groupedTraderMatches = queryReportMatches.GroupBy(x => x.TraderName).Select(y => y.ToList()).ToList();

        var reportLeague = buildTraderReportLeague(groupedTraderLeague).ToList();
        var reportMatches = buildTraderReportMatches(groupedTraderMatches).ToList();

        foreach (var element in reportLeague)
        {
            var findTrader = reportMatches.FirstOrDefault(x => x.TraderName == element.TraderName);
            element.ReportHDPMatches = findTrader.ReportHDPMatches;
            element.ReportOUMatches = findTrader.ReportOUMatches;
            element.TotalMatches = findTrader.TotalMatches;
        }

        return reportLeague;
    }
    public IList<ReportModel> buildTraderReportLeague(List<List<QueryPrematchReportModel>> traderReportLst)
    {
        var traderOfLeague = new List<ReportModel>();

        foreach (var item in traderReportLst)
        {
            var hdpLeague = buildSingleTraderHDPLeague(item);
            var ouLeague = buildSingleTraderOULeague(item);
            var temp = new ReportModel
            {
                TraderName = item[0].TraderName,
                ReportHDPLeague = hdpLeague,
                ReportOULeague = ouLeague,
                TotalLeague = hdpLeague.Total + ouLeague.Total
            };

            traderOfLeague.Add(temp);
        }
        return traderOfLeague;
    }
    public IList<ReportModel> buildTraderReportMatches(List<List<QueryPrematchReportModel>> traderReportLst)
    {
        var traderOfLeague = new List<ReportModel>();

        foreach (var item in traderReportLst)
        {
            var hdpMatches = buildSingleTraderHDPMatches(item);
            var ouMatches = buildSingleTraderOUMatches(item);
            var temp = new ReportModel
            {
                TraderName = item[0].TraderName,
                ReportHDPMatches = hdpMatches,
                ReportOUMatches = ouMatches,
                TotalMatches = hdpMatches.Total + ouMatches.Total
            };

            traderOfLeague.Add(temp);
        }
        return traderOfLeague;
    }
    public ReportHDPLeagueModel buildSingleTraderHDPLeague(IList<QueryPrematchReportModel> reportItem)
    {
        var singleTraderOfLeague = new ReportHDPLeagueModel();
        var HDPLeagueLst = reportItem.Select(x => x).Where(y => y.Bettype == 1).ToList();
        foreach (var element in HDPLeagueLst)
        {
            BuildHelper.build(singleTraderOfLeague, element);
            singleTraderOfLeague.Total = HDPLeagueLst.Sum(x => x.Amount);
        }
        return singleTraderOfLeague;
    }
    public ReportOULeagueModel buildSingleTraderOULeague(IList<QueryPrematchReportModel> reportItem)
    {
        var singleTraderOfLeague = new ReportOULeagueModel();
        var OULeagueLst = reportItem.Select(x => x).Where(y => y.Bettype == 2).ToList();
        foreach (var element in OULeagueLst)
        {
            BuildHelper.build(singleTraderOfLeague, element);
            singleTraderOfLeague.Total = OULeagueLst.Sum(x => x.Amount);
        }
        return singleTraderOfLeague;
    }
    public ReportHDPMatchesModel buildSingleTraderHDPMatches(IList<QueryPrematchReportModel> reportItem)
    {
        var singleTraderOfMatches = new ReportHDPMatchesModel();
        var HDPMatchesLst = reportItem.Select(x => x).Where(y => y.Bettype == 1).ToList();
        foreach (var element in HDPMatchesLst)
        {
            BuildHelper.build(singleTraderOfMatches, element);
            singleTraderOfMatches.Total = HDPMatchesLst.Sum(x => x.Amount);
        }
        return singleTraderOfMatches;
    }
    public ReportOUMatchesModel buildSingleTraderOUMatches(IList<QueryPrematchReportModel> reportItem)
    {
        var singleTraderOfMatches = new ReportOUMatchesModel();
        var OUMatchesLst = reportItem.Select(x => x).Where(y => y.Bettype == 2).ToList();
        foreach (var element in OUMatchesLst)
        {
            BuildHelper.build(singleTraderOfMatches, element);
            singleTraderOfMatches.Total = OUMatchesLst.Sum(x => x.Amount);
        }
        return singleTraderOfMatches;
    }
}