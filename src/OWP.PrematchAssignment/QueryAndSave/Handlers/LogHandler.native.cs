using MediatR;
using OWP.PrematchAssignment.QueryAndSave.Models;
using OWP.PrematchAssignment.QueryAndSave.Services;

namespace OWP.PrematchAssignment.QueryAndSave.Handlers;

public class QueryLogRequest : IRequest<QueryLogViewModel>
{
    public int PageSize { get; set; }

    public int PageNumber { get; set; }

    public bool IsDay { get; set; }
}

public class LogHandler : IRequestHandler<QueryLogRequest, QueryLogViewModel>
{
    private readonly QueryLogService queryLogService;

    public LogHandler(QueryLogService queryLogService)
    {
        this.queryLogService = queryLogService;
    }

    public async Task<QueryLogViewModel> Handle(QueryLogRequest request, CancellationToken cancellationToken)
    {
        return await queryLogService.Query(request);
    }
}