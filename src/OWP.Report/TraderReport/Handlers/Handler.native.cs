namespace OWP.Report.TraderReport.Handlers;

using Constants;
using MediatR;
using Models;
using Report.Models;
using Services;

public class ReportRequest : QueryModel, IRequest<IEnumerable<ReportModel>>
{
}

public class Handler : IRequestHandler<ReportRequest, IEnumerable<ReportModel>>
{
    private readonly RepositoryService repository;
    private readonly TGRSReportService tGRSReportService;

    public Handler(RepositoryService repository, TGRSReportService tGRSReportService)
    {
        this.repository = repository;
        this.tGRSReportService = tGRSReportService;
    }

    public async Task<IEnumerable<ReportModel>> Handle(ReportRequest request, CancellationToken cancellationToken)
    {
        var dataReport = await repository.GetLstReportDataByDate(request);
        var traderIds = dataReport.Select(x => x.UserId).Distinct().ToList();
        var traderPerformanceDetails = await tGRSReportService.GetTraderPerformanceDetails(traderIds, request);

        AssignTraderPerformanceDetail(dataReport, traderPerformanceDetails);

        return dataReport.OrderBy(x => x.SectionName).ThenBy(y => y.Username);
    }

    private static void AssignTraderPerformanceDetail(IEnumerable<ReportModel> dataReport, IEnumerable<TraderPerformanceDetail> traderPerformanceDetails)
    {
        foreach (var item in dataReport)
        {
            const int bettypeOther = 0;
            var traders = traderPerformanceDetails.Where(x => x.TraderId == item.UserId).ToArray();
            item.TurnOver_FT_HDP = traders.Where(x => x.BetType == Bettype.Handicap).Sum(x => x.Turnover);
            item.TurnOver_FT_OU = traders.Where(x => x.BetType == Bettype.OverUnder).Sum(x => x.Turnover);
            item.TurnOver_1H_HDP = traders.Where(x => x.BetType == Bettype.FirstHalfHandicap).Sum(x => x.Turnover);
            item.TurnOver_1H_OU = traders.Where(x => x.BetType == Bettype.FirstHalfOverUnder).Sum(x => x.Turnover);
            item.TurnOver_Others = traders.Where(x => x.BetType == bettypeOther).Sum(x => x.Turnover);

            item.WinLoss_FT_HDP = traders.Where(x => x.BetType == Bettype.Handicap).Sum(x => x.WinLoss);
            item.Winloss_FT_OU = traders.Where(x => x.BetType == Bettype.OverUnder).Sum(x => x.WinLoss);
            item.Winloss_1H_HDP = traders.Where(x => x.BetType == Bettype.FirstHalfHandicap).Sum(x => x.WinLoss);
            item.Winloss_1H_OU = traders.Where(x => x.BetType == Bettype.FirstHalfOverUnder).Sum(x => x.WinLoss);
            item.Winloss_Others = traders.Where(x => x.BetType == bettypeOther).Sum(x => x.WinLoss);
        }
    }
}