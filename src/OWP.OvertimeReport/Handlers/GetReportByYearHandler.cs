namespace OWP.OvertimeReport.Handlers
{
    using AutoMapper;
    using MediatR;
    using Models;
    using Services;

    public class QueryReportByYearRequest : IRequest<IEnumerable<OvertimeByYear>>
    {
        public string Location { get; set; }

        public string Trader { get; set; }

        public int Year { get; set; }
    }

    public class GetReportByYearHandler : IRequestHandler<QueryReportByYearRequest, IEnumerable<OvertimeByYear>>
    {
        private readonly CommandRepository commandRepository;

        private readonly IMapper mapper;

        public GetReportByYearHandler(CommandRepository commandRepository, IMapper mapper)
        {
            this.commandRepository = commandRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<OvertimeByYear>> Handle(QueryReportByYearRequest request, CancellationToken cancellationToken)
        {
            var getRequest = mapper.Map<OverTimeRequest>(request);
            var (users, overtimeData) = await commandRepository.GetReportByYear(getRequest);
            var dataGroupedByTrader = overtimeData.GroupBy(x => x.UserId, (key, g) =>
                new GroupedData { UserId = key, Data = g.ToList() });

            var res = CalculateMonthlyReport(dataGroupedByTrader, users);

            return res;
        }

        private IEnumerable<OvertimeByYear> CalculateMonthlyReport(IEnumerable<GroupedData> dataGroupedByTrader, IEnumerable<User> users)
        {
            var result = new List<OvertimeByYear>();

            foreach (var trader in users)
            {
                var traderData = dataGroupedByTrader.FirstOrDefault(x => x.UserId == trader.UserId);

                if (traderData != null)
                {
                    var traderOvertimeData = CalculateTraderMonthlyHours(traderData.Data);
                    var user = users.FirstOrDefault(x => x.UserId == traderData.UserId);

                    result.Add(new OvertimeByYear
                    {
                        Trader = user?.Username,
                        OvertimeData = traderOvertimeData.ToList(),
                        Total = traderOvertimeData.Sum(),
                        Location = user?.Location
                    });
                }
                else
                {
                    result.Add(new OvertimeByYear
                    {
                        Location = trader.Location,
                        OvertimeData = new List<decimal?> { null, null, null, null, null, null, null, null, null, null, null, null },
                        Total = null,
                        Trader = trader.Username
                    });
                }
            }

            return result.OrderBy(x => x.Location).ThenBy(x => x.Trader).ToList();
        }

        private IEnumerable<decimal?> CalculateTraderMonthlyHours(IEnumerable<OvertimeData> data)
        {
            for (var i = 1; i <= 12; i++)
            {
                yield return data.Any(x => x.ReportDate.Month == i) ? data.Where(x => x.ReportDate.Month == i).Sum(x => x.Hour) : null;
            }
        }

        private class GroupedData
        {
            public int UserId { get; set; }

            public List<OvertimeData> Data { get; set; }
        }
    }
}