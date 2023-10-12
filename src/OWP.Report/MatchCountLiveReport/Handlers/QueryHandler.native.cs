using MediatR;
using OWP.Report.MatchCountLiveReport.Criteria;
using OWP.Report.MatchCountLiveReport.Models;

namespace OWP.Report.MatchCountLiveReport.Handlers;

public class QueryRequest : IRequest<IList<ReportInfo>>
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }
}

public class QueryHandler : IRequestHandler<QueryRequest, IList<ReportInfo>>
{
    private readonly Repository repository;

    public QueryHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<IList<ReportInfo>> Handle(QueryRequest request, CancellationToken cancellationToken)
    {

        var criteria = new QueryReportInfoCriteria
        {
            FromDate = request.FromDate,
            ToDate = request.ToDate
        };
        return await repository.QueryReportInfo(criteria);
    }

}