using Fanex.Data.Repository;
using MediatR;
using OWP.TicketEvent.Criteria;
using OWP.TicketEvent.Models;

namespace OWP.TicketEvent.Handlers;

public class GetTicketListRequest : IRequest<IList<TicketListModel>>
{
    public int OwpMatchId { get; set; }
}

public class GetTicketListHandler : IRequestHandler<GetTicketListRequest, IList<TicketListModel>>
{
    private readonly Repository repository;

    public GetTicketListHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<IList<TicketListModel>> Handle(GetTicketListRequest request, CancellationToken cancellationToken)
    {
        var ticketList = await repository.GetTicketFromDb(request.OwpMatchId);
        return BuildResponseTicket(ticketList);
    }

    private IList<TicketListModel> BuildResponseTicket(IList<TicketListModel> models)
    {
        var ticketResponse = new List<TicketListModel>();
        var ticketPair = models
            .GroupBy(t => t.TicketId)
            .ToDictionary(t => t.Key, t => t.ToList().OrderByDescending(x => x.CreatedDate).First());

        foreach (var ticket in ticketPair)
        {
            ticketResponse.Add(ticket.Value);
        }
        return ticketResponse;
    }
}
