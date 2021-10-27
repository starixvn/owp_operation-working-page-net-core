namespace OWP.OvertimeReport.Handlers
{
    using Fanex.Data.Repository;
    using MediatR;
    using Models;
    using OWP.OvertimeReport.Criteria;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class QueryReportByYearRequest : IRequest<IEnumerable<OvertimeByYear>>
    {
        public string Location { get; set; }

        public string Trader { get; set; }

        public int Year { get; set; }
    }

    public class GetReportByYearHandler : IRequestHandler<QueryReportByYearRequest, IEnumerable<OvertimeByYear>>
    {
        private readonly IDynamicRepository repository;

        public GetReportByYearHandler(IDynamicRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<OvertimeByYear>> Handle(QueryReportByYearRequest request, CancellationToken cancellationToken)
        {
            var (users, overtimeData) = await GetReportByYear(request);
            var dataGroupedByTrader = overtimeData.GroupBy(x => x.UserId, (key, g) => new GroupedData { UserId = key, Data = g.ToList() });

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

        private async Task<(IEnumerable<User>, IEnumerable<OvertimeData>)> GetReportByYear(QueryReportByYearRequest request)
        {
            var minDate = new DateTime(request.Year, 1, 1);
            var maxDate = new DateTime(request.Year, 12, 31);
            var criteria = new GetReportByDateRangeCriteria
            {
                TraderName = request.Trader ?? string.Empty,
                Location = request.Location ?? string.Empty,
                FromDate = minDate,
                ToDate = maxDate
            };

            return await repository.FetchMultipleAsync<User, OvertimeData>(criteria);
        }

        private class GroupedData
        {
            public int UserId { get; set; }

            public List<OvertimeData> Data { get; set; }
        }
    }
}