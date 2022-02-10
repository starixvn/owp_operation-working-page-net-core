namespace OWP.PreMatchReport.Handlers
{
    using AutoMapper;
    using OWP.PreMatchReport.Helper;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using OWP.PreMatchReport.Models;
    using MediatR;

    public class ReportRequest : QueryModel, IRequest<IEnumerable<ReportModel>>
    {

    }
    public class ReportHandler : IRequestHandler<ReportRequest, IEnumerable<ReportModel>>
    {
        private readonly IMapper mapper;
        private readonly Repository repository;

        public ReportHandler(IMapper mapper, Repository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<IEnumerable<ReportModel>> Handle(ReportRequest request, CancellationToken cancellationToken)
        {
            var queryReport = await repository.GetReportForLeague(request);
            var groupedTraderLeague = queryReport.GroupBy(x => x.TraderName).Select(y => y.ToList()).ToList();
            var reportLeague = buildTraderReportLeague(groupedTraderLeague).ToList();
            var reportPreMatch = mapper.Map<IList<ReportModel>>(reportLeague);           
            return reportPreMatch;
        }
        public IList<ReportLeagueModel> buildTraderReportLeague(List<List<QueryPrematchReportModel>> traderReportLst)
        {
            var traderOfLeague = new List<ReportLeagueModel>();

            foreach (var item in traderReportLst)
            {
                var temp = buildSingleTraderLeague(item);
                traderOfLeague.Add(temp);
            }
            return traderOfLeague;
        }
        public ReportLeagueModel buildSingleTraderLeague(IList<QueryPrematchReportModel> reportItem)
        {
            var singleTraderOfLeague = new ReportLeagueModel();
            var HDPLeagueLst = reportItem.Select(x => x).Where(y => y.Bettype == 1).ToList();
            var OULeagueLst = reportItem.Select(x => x).Where(y => y.Bettype == 2).ToList();
            foreach (var element in HDPLeagueLst)
            {
                singleTraderOfLeague.TraderName = element.TraderName;
                HDPLeagueHelper.buildHDPLeague(singleTraderOfLeague, element);
                singleTraderOfLeague.Total_HDPLeague = HDPLeagueLst.Sum(x=>x.Amount);
            }
            foreach (var element in OULeagueLst)
            {
                HDPLeagueHelper.buildOULeague(singleTraderOfLeague, element);
                singleTraderOfLeague.Total_OULeague = OULeagueLst.Sum(x=>x.Amount);
            }
            return singleTraderOfLeague;
        }
    }
}