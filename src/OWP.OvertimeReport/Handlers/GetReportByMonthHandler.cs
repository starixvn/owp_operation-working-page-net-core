namespace OWP.OvertimeReport.Handlers
{
    using MediatR;
    using Models;
    using Services;
    using System.Collections.Generic;
    using System.Linq;
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
            var result = await commandRepository.GetReportByMonth();

            return result.GroupBy(x => new { x.Location, x.Trader }, (key, values) =>
                new OvertimeByMonth
                {
                    Location = key.Location,
                    Trader = key.Trader,
                    Overtime = values.Select(x => new OverTimeInfo
                    {
                        Date = x.Date,
                        IsOffday = x.IsOffday,
                        IsWeekend = x.IsWeekend,
                        DayOfWeek = x.DayOfWeek,
                        Hour = x.Hour
                    })
                    .OrderBy(x => x.Date)
                    .ToList()
                }
            )
            .OrderBy(x => x.Location)
            .ThenBy(x => x.Trader);
        }
    }
}
