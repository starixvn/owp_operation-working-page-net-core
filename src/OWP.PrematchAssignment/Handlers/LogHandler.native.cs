namespace OWP.PreMatchAssignment.Handlers
{
    using MediatR;
    using Models;
    using Services;

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
}