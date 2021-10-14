namespace OWP.OvertimeReport.Handlers
{
    using MediatR;
    using Models;
    using Services;
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
        private readonly CommandRepository commandRepository;

        public GetReportByYearHandler(CommandRepository commandRepository)
        {
            this.commandRepository = commandRepository;
        }

        public async Task<IEnumerable<OvertimeByYear>> Handle(QueryReportByYearRequest request, CancellationToken cancellationToken)
        {
            var result = await commandRepository.GetReportByMonth();

            return result.GroupBy(x => new { x.Location, x.Trader }, (key, values) =>
                new OvertimeByYear
                {

                }
            );
        }
    }
}
