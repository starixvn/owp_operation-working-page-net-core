namespace OWP.TicketCenter.Services;

using Handlers;
using Models;

public interface GetEventTicketsService
{
    Task<IList<TicketBaseInfo>> GetAllTickets(QueryEventTicketsRequest request);
}

public class GetEventTicketsServiceImp : GetEventTicketsService
{
    private readonly Repository repository;

    public GetEventTicketsServiceImp(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<IList<TicketBaseInfo>> GetAllTickets(QueryEventTicketsRequest request)
    {
        var ticketLeague = await repository.GetEventTicketLeague(request);
        var ticketMatch = await repository.GetEventTicketMatch(request);
        var ticketOthers = await repository.GetEventTicketOther(request);
        var ticketSupport = await repository.GetEventTicketSupport(request);
        var ticketLiveIncident = await repository.GetEventTicketLiveIncident(request);
        var ticketIdSaved = await repository.GetTicketIdHaveSavedMessage();
        var eventTickets = new List<TicketBaseInfo>();
        eventTickets.AddRange(ticketLeague);
        eventTickets.AddRange(ticketMatch);
        eventTickets.AddRange(ticketOthers);
        eventTickets.AddRange(ticketSupport);
        eventTickets.AddRange(ticketLiveIncident);

        var ticketIsShowed = BuildTicketIsShowed(eventTickets, ticketIdSaved.ToList());

        var ticketIds = ticketIsShowed.Select(x => x.TicketId).ToList();
        var lastComments = await repository.GetTwoLastComment(ticketIds);

        foreach (var ticket in ticketIsShowed)
        {
            ticket.Messages = lastComments.Where(x => x.TicketId == ticket.TicketId).ToList();
        }

        return SortEventTicketByCommentLatestDate(ticketIsShowed);
    }

    private List<TicketBaseInfo> SortEventTicketByCommentLatestDate(
        List<TicketBaseInfo> tickets)
        => tickets.OrderByDescending(x => x.IsPinned).ThenByDescending(x => x.LastCommentDate).ToList();

    private List<TicketBaseInfo> BuildTicketIsShowed(List<TicketBaseInfo> allTicket, List<int> ticketId)
    {
        var ticketSaved = new List<TicketBaseInfo>();

        ticketId.ToList().ForEach(x =>
        {
            var findTicket = allTicket.Where(y => y.TicketId.Equals(x)).ToList();
            ticketSaved.AddRange(findTicket);
        });

        var ticketNotSaved = allTicket.Except(ticketSaved).ToList();

        var ticketIsShowed = ticketNotSaved.Where(x => DateTime.Now.Subtract(x.LastCommentDate).Days <= 30).ToList();

        ticketSaved.AddRange(ticketIsShowed);

        return ticketSaved;
    }
}