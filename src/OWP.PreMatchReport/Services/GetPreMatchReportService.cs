using AutoMapper;
using OWP.PreMatchReport.Constants;
using OWP.PreMatchReport.Helper;
using OWP.PreMatchReport.Models;

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

        var groupedTraderLeague = GroupTraderByUserName(queryReportLeague);
        var groupedTraderMatches = GroupTraderByUserName(queryReportMatches);

        var reportLeague = BuildTraderReportLeague(groupedTraderLeague);
        var reportMatches = BuildTraderReportMatches(groupedTraderMatches);

        foreach (var element in reportLeague)
        {
            var findTrader = reportMatches.FirstOrDefault(x => x.TraderName == element.TraderName);
            element.ReportHDPMatches = findTrader.ReportHDPMatches;
            element.ReportOUMatches = findTrader.ReportOUMatches;
            element.TotalMatches = findTrader.TotalMatches;
        }

        List<int> assignmentUserId = reportLeague.Select(x => x.UserId).ToList();

        var userNames = await repository.GetUnAssigned(assignmentUserId, queryModel.KeyWord);

        var reportEmpty = userNames.Select(item => new ReportModel
        {
            TraderName = item,
            ReportHDPLeague = new ReportHDPLeagueModel(),
            ReportHDPMatches = new ReportHDPMatchesModel(),
            ReportOULeague = new ReportOULeagueModel(),
            ReportOUMatches = new ReportOUMatchesModel()
        }).ToList();

        return mapper.Map<List<ExportPreMatchModel>>(reportLeague.Concat(reportEmpty));
    }

    public Dictionary<string, List<QueryPrematchReportModel>> GroupTraderByUserName(IEnumerable<QueryPrematchReportModel> queryReport)
    => queryReport.GroupBy(x => x.TraderName).ToDictionary(t => t.Key, t => t.Select(r => r).ToList());

    public IList<ReportModel> BuildTraderReportLeague(Dictionary<string, List<QueryPrematchReportModel>> traderReport)
    {
        var traderOfLeague = new List<ReportModel>();

        foreach (var item in traderReport)
        {
            var hdpLeague = new ReportHDPLeagueModel();
            BuildSingleTraderHDP(item.Value, hdpLeague);
            var ouLeague = new ReportOULeagueModel();
            BuildSingleTraderOU(item.Value, ouLeague);
            var temp = new ReportModel
            {
                UserId = item.Value[0].UserId,
                TraderName = item.Key,
                ReportHDPLeague = hdpLeague,
                ReportOULeague = ouLeague,
                TotalLeague = hdpLeague.Total + ouLeague.Total
            };

            traderOfLeague.Add(temp);
        }
        return traderOfLeague;
    }

    public IList<ReportModel> BuildTraderReportMatches(Dictionary<string, List<QueryPrematchReportModel>> traderReport)
    {
        var traderOfLeague = new List<ReportModel>();

        foreach (var item in traderReport)
        {
            var hdpMatches = new ReportHDPMatchesModel();
            BuildSingleTraderHDP(item.Value, hdpMatches);
            var ouMatches = new ReportOUMatchesModel();
            BuildSingleTraderOU(item.Value, ouMatches);
            var temp = new ReportModel
            {
                UserId = item.Value[0].UserId,
                TraderName = item.Key,
                ReportHDPMatches = hdpMatches,
                ReportOUMatches = ouMatches,
                TotalMatches = hdpMatches.Total + ouMatches.Total
            };

            traderOfLeague.Add(temp);
        }
        return traderOfLeague;
    }

    public void BuildSingleTraderHDP(IList<QueryPrematchReportModel> reportItem, ReportBaseModel singleTraderOfLeague)
    {
        var HDPLst = reportItem.Select(x => x).Where(y => y?.Bettype == ReportConst.HDP_Id).ToList();
        foreach (var element in HDPLst)
        {
            LeagueGroupHelper.Build(singleTraderOfLeague, element);
            singleTraderOfLeague.Total = HDPLst.Sum(x => x.Amount);
        }
    }

    public void BuildSingleTraderOU(IList<QueryPrematchReportModel> reportItem, ReportBaseModel singleTraderOfLeague)
    {
        var OULst = reportItem.Select(x => x).Where(y => y.Bettype == ReportConst.OU_Id).ToList();
        foreach (var element in OULst)
        {
            LeagueGroupHelper.Build(singleTraderOfLeague, element);
            singleTraderOfLeague.Total = OULst.Sum(x => x.Amount);
        }
    }
}