using Fanex.Data.Repository;
using MediatR;
using OWP.TicketEvent.Criteria;
using OWP.TicketEvent.Models;

namespace OWP.TicketEvent.Handlers;

public class GetTicketListRequest : IRequest<IList<TicketListModel>>
{
    public int OWPMatchId { get; set; }
}

public class GetTicketListHandler : IRequestHandler<GetTicketListRequest, IList<TicketListModel>>
{
    private readonly IDynamicRepository repository;

    public GetTicketListHandler(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IList<TicketListModel>> Handle(GetTicketListRequest request, CancellationToken cancellationToken)
    {
        var tiektList = await GetTicketFromDB(request.OWPMatchId);
        return BuildResponseTicket(tiektList);
    }

    private async Task<IList<TicketListModel>> GetTicketFromDB(int OWPMatchId)
    => (await repository.FetchAsync<TicketListModel>(new GetTicketListCriteria
    {
        OWPMatchId = OWPMatchId
    })).ToList();

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
