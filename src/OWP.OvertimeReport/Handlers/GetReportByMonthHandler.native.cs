namespace OWP.OvertimeReport.Handlers
{
    using AutoMapper;
    using MediatR;
    using Models;
    using Services;
    using System.Threading;
    using System.Threading.Tasks;

    public class QueryReportByMonthRequest : IRequest<OvertimeByMonth>
    {
        public string Location { get; set; }

        public string Trader { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }
    }

    public class GetReportByMonthHandler : IRequestHandler<QueryReportByMonthRequest, OvertimeByMonth>
    {
        private readonly IMapper mapper;
        private readonly ReportByMonthServices reportService;

        public GetReportByMonthHandler(
            IMapper mapper,
            ReportByMonthServices reportService)
        {
            this.mapper = mapper;
            this.reportService = reportService;
        }

        public async Task<OvertimeByMonth> Handle(QueryReportByMonthRequest request, CancellationToken cancellationToken)
        {
            var getRequest = mapper.Map<OverTimeByMonthRequest>(request);

            return await reportService.GetReportByMonth(getRequest);
        }
    }
}
