using OWP.PreMatchReport.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OWP.PreMatchReport.Models;
using OWP.PreMatchReport.Handlers;
using OWP.PreMatchReport.Constants;
using MediatR;

namespace OWP.PreMatchReport.Services;

public interface GetPreMatchReportService
{
    Task<IList<ReportModel>> GetReports(QueryModel queryModel);
}

public class GetPreMatchReportServiceImp : GetPreMatchReportService
{
    private readonly Repository repository;

    public GetPreMatchReportServiceImp(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<IList<ReportModel>> GetReports(QueryModel queryModel)
    {
        var queryReportLeague = await repository.GetReportForLeague(queryModel);
        var queryReportMatches = await repository.GetReportForMatches(queryModel);

        var groupedTraderLeague = groupTraderByUserName(queryReportLeague);
        var groupedTraderMatches = groupTraderByUserName(queryReportMatches);

        var reportLeague = buildTraderReportLeague(groupedTraderLeague);
        var reportMatches = buildTraderReportMatches(groupedTraderMatches);

        foreach (var element in reportLeague)
        {
            var findTrader = reportMatches.FirstOrDefault(x => x.TraderName == element.TraderName);
            element.ReportHDPMatches = findTrader.ReportHDPMatches;
            element.ReportOUMatches = findTrader.ReportOUMatches;
            element.TotalMatches = findTrader.TotalMatches;
        }

        return reportLeague;
    }

    public Dictionary<string, List<QueryPrematchReportModel>> groupTraderByUserName(IEnumerable<QueryPrematchReportModel> queryReport)
    => queryReport.GroupBy(x => x.TraderName).ToDictionary(t => t.Key, t => t.Select(r => r).ToList());

    public IList<ReportModel> buildTraderReportLeague(Dictionary<string, List<QueryPrematchReportModel>> traderReport)
    {
        var traderOfLeague = new List<ReportModel>();

        foreach (var item in traderReport)
        {
            var hdpLeague = new ReportHDPLeagueModel();
            buildSingleTraderHDP(item.Value, hdpLeague);
            var ouLeague = new ReportOULeagueModel();
            buildSingleTraderOU(item.Value, ouLeague);
            var temp = new ReportModel
            {
                TraderName = item.Key,
                ReportHDPLeague = hdpLeague,
                ReportOULeague = ouLeague,
                TotalLeague = hdpLeague.Total + ouLeague.Total
            };

            traderOfLeague.Add(temp);
        }
        return traderOfLeague;
    }
    public IList<ReportModel> buildTraderReportMatches(Dictionary<string, List<QueryPrematchReportModel>> traderReport)
    {
        var traderOfLeague = new List<ReportModel>();

        foreach (var item in traderReport)
        {
            var hdpMatches = new ReportHDPMatchesModel();
            buildSingleTraderHDP(item.Value, hdpMatches);
            var ouMatches = new ReportOUMatchesModel();
            buildSingleTraderOU(item.Value, ouMatches);
            var temp = new ReportModel
            {
                TraderName = item.Key,
                ReportHDPMatches = hdpMatches,
                ReportOUMatches = ouMatches,
                TotalMatches = hdpMatches.Total + ouMatches.Total
            };

            traderOfLeague.Add(temp);
        }
        return traderOfLeague;
    }
    public void buildSingleTraderHDP(IList<QueryPrematchReportModel> reportItem, ReportBaseModel singleTraderOfLeague)
    {
        var HDPLst = reportItem.Select(x => x).Where(y => y.Bettype == ReportConst.HDP_Id).ToList();
        foreach (var element in HDPLst)
        {
            BuildHelper.build(singleTraderOfLeague, element);
            singleTraderOfLeague.Total = HDPLst.Sum(x => x.Amount);
        }
    }
    public void buildSingleTraderOU(IList<QueryPrematchReportModel> reportItem, ReportBaseModel singleTraderOfLeague)
    {
        var OULst = reportItem.Select(x => x).Where(y => y.Bettype == ReportConst.OU_Id).ToList();
        foreach (var element in OULst)
        {
            BuildHelper.build(singleTraderOfLeague, element);
            singleTraderOfLeague.Total = OULst.Sum(x => x.Amount);
        }


    }
}