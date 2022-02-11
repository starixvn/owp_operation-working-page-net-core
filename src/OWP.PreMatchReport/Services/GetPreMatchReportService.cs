using OWP.PreMatchReport.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using OWP.PreMatchReport.Models;
using OWP.PreMatchReport.Handlers;
using OWP.PreMatchReport.Constants;
using MediatR;

namespace OWP.PreMatchReport.Services;

public interface GetPreMatchReportService
{
    Task<IList<ExportPreMatchModel>> GetReports(QueryModel queryModel);
}

public class GetPreMatchReportServiceImp : GetPreMatchReportService
{
    private readonly Repository repository;
    private readonly IMapper mapper;

    public GetPreMatchReportServiceImp(Repository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<IList<ExportPreMatchModel>> GetReports(QueryModel queryModel)
    {
        var queryReportLeague = await repository.GetReportForLeague(queryModel);
        var queryReportMatches = await repository.GetReportForMatches(queryModel);

        var groupedTraderLeague = groupTraderByUserName(queryReportLeague);
        var groupedTraderMatches = groupTraderByUserName(queryReportMatches);

        var reportLeague = BuildTraderReportLeague(groupedTraderLeague).ToList();
        var reportMatches = BuildTraderReportMatches(groupedTraderMatches).ToList();

        foreach (var element in reportLeague)
        {
            var findTrader = reportMatches.FirstOrDefault(x => x.TraderName == element.TraderName);
            element.ReportHDPMatches = findTrader.ReportHDPMatches;
            element.ReportOUMatches = findTrader.ReportOUMatches;
            element.TotalMatches = findTrader.TotalMatches;
        }

        return mapper.Map<List<ExportPreMatchModel>>(reportLeague);
    }

    public IList<ReportModel> BuildTraderReportLeague(List<List<QueryPrematchReportModel>> traderReportLst)
    {
        var traderOfLeague = new List<ReportModel>();

        foreach (var item in traderReport)
        {
            var hdpLeague = BuildSingleTraderHDPLeague(item);
            var ouLeague = BuildSingleTraderOULeague(item);
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

    public IList<ReportModel> BuildTraderReportMatches(List<List<QueryPrematchReportModel>> traderReportLst)
    {
        var traderOfLeague = new List<ReportModel>();

        foreach (var item in traderReport)
        {
            var hdpMatches = BuildSingleTraderHDPMatches(item);
            var ouMatches = BuildSingleTraderOUMatches(item);
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

    public ReportHDPLeagueModel BuildSingleTraderHDPLeague(IList<QueryPrematchReportModel> reportItem)
    {
        var HDPLst = reportItem.Select(x => x).Where(y => y.Bettype == ReportConst.HDP_Id).ToList();
        foreach (var element in HDPLst)
        {
            BuildHelper.build(singleTraderOfLeague, element);
            singleTraderOfLeague.Total = HDPLst.Sum(x => x.Amount);
        }
    }

    public ReportOULeagueModel BuildSingleTraderOULeague(IList<QueryPrematchReportModel> reportItem)
    {
        var OULst = reportItem.Select(x => x).Where(y => y.Bettype == ReportConst.OU_Id).ToList();
        foreach (var element in OULst)
        {
            BuildHelper.build(singleTraderOfLeague, element);
            singleTraderOfLeague.Total = OULst.Sum(x => x.Amount);
        }

    }
}