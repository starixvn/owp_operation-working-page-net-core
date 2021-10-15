namespace OWP.OvertimeReport.Handlers
{
    using MediatR;
    using Models;
    using Services;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class QueryReportByMonthRequest : IRequest<IEnumerable<OvertimeByMonth>>
    {
        public string Location { get; set; }

        public string Trader { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }
    }

    public class GetReportByMonthHandler : IRequestHandler<QueryReportByMonthRequest, IEnumerable<OvertimeByMonth>>
    {
        private readonly CommandRepository commandRepository;

        public GetReportByMonthHandler(CommandRepository commandRepository)
        {
            this.commandRepository = commandRepository;
        }

        public async Task<IEnumerable<OvertimeByMonth>> Handle(QueryReportByMonthRequest request, CancellationToken cancellationToken)
        {
            return new List<OvertimeByMonth>();
        }
    }
}
