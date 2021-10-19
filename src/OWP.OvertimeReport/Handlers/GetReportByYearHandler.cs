namespace OWP.OvertimeReport.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using System.Linq;
    using MediatR;
    using Models;
    using OWP.OvertimeReport.Criteria;

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
            var dataGroupedByTrader = overtimeData.GroupBy(x => x.Username, (key, g) => new { Username = key, Data = g.ToList()});

            foreach (var traderData in dataGroupedByTrader)
            {
                var traderOvertimeData = CalculateMonthlyReport(traderData.Data);

                yield return new OvertimeByYear
                {
                    Trader = traderData.Username,
                    OvertimeData = traderOvertimeData,
                    Total = traderOvertimeData.Sum(),
                    Location = string.Empty
                };
            }
        }

        private List<decimal?> CalculateMonthlyReport(IEnumerable<OvertimeData> data)
        {
            
        }

        private async Task<(IEnumerable<User>, IEnumerable<OvertimeData>)> GetReportByYear(QueryReportByYearRequest request)
        {
            var minDate = new DateTime(request.Year, 1, 1);
            var maxDate = new DateTime(request.Year, 12, 31);
            var criteria = new GetReportByDateRangeCriteria
            {
                TraderName = request.Trader,
                Location = request.Location,
                FromDate = minDate,
                ToDate = maxDate
            };

            return await repository.FetchMultipleAsync<User, OvertimeData>(criteria);
        }
    }
}