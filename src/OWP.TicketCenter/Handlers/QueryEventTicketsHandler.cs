namespace OWP.TicketCenter.Handlers
{
    using MediatR;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class QueryEventTicketsRequest : IRequest<IEnumerable<object>>
    {
        public string EventKey { get; set; }

        public int TicketTypeKey { get; set; }

        public int TicketIdKey { get; set; }

        public int MarketKey { get; set; }
    }

    public class QueryEventTicketsHandler : IRequestHandler<QueryEventTicketsRequest, IEnumerable<object>>
    {
        private readonly Repository repository;

        public QueryEventTicketsHandler(Repository repository)
        {
            this.repository = repository;
        }

        public Task<IEnumerable<object>> Handle(QueryEventTicketsRequest request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}