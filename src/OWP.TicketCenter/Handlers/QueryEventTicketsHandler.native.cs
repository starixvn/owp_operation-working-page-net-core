using Olympus.Xml;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Services;

namespace OWP.TicketCenter.Handlers
{
    using MediatR;

    public class QueryEventTicketsRequest : IRequest<IList<TicketBaseInfo>>
    {
        public int ActorId { get; set; }

        public string Keyword { get; set; }

        public string TicketTypeId { get; set; }

        public string TicketStatuses { get; set; }

        public string TicketId { get; set; }

        public int MarketKey { get; set; }

        public string TicketTypeXML => GetTicketTypeXML(TicketTypeId);

        public string GetTicketTypeXML(string ticketTypeString)
        {
            var ticket = ticketTypeString.Split(',')
                .Select(i => new TicketType
                {
                    TypeId = i.Split('-').FirstOrDefault(),
                    SubTypeId = i.Split('-').LastOrDefault()
                });
            return XmlConverter.Serialize(ticket);
        }
    }

    public class QueryEventTicketsHandler : IRequestHandler<QueryEventTicketsRequest, IList<TicketBaseInfo>>
    {
        private readonly GetEventTicketsService getEventTicketService;

        public QueryEventTicketsHandler(GetEventTicketsService getEventTicketService)
        {
            this.getEventTicketService = getEventTicketService;
        }

        public async Task<IList<TicketBaseInfo>> Handle(QueryEventTicketsRequest request, CancellationToken cancellationToken)
             => await getEventTicketService.GetAllTickets(request);
    }
}