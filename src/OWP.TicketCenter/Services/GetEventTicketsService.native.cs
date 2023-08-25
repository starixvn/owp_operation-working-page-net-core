using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Services;

public interface GetEventTicketsService
{
    Task<IList<TicketInfo>> GetAllTickets(QueryEventTicketsRequest request);
}

public class GetEventTicketsServiceImp : GetEventTicketsService
{
    private readonly Repository repository;

    public GetEventTicketsServiceImp(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<IList<TicketInfo>> GetAllTickets(QueryEventTicketsRequest request)
    {
        var eventTickets = await GetEventTickets(request);
        var ticketIds = eventTickets.Select(x => x.TicketId).ToList();

        await SetLastComment(ticketIds, eventTickets);

        return SortEventTicketByCommentLatestDate(eventTickets);
    }

    private async Task<List<TicketInfo>> GetEventTickets(QueryEventTicketsRequest request)
    {
        var ticketLeague = repository.GetEventTicketLeague(request);
        var ticketMatch = repository.GetEventTicketMatch(request);
        var ticketOthers = repository.GetEventTicketOther(request);
        var ticketSupport = repository.GetEventTicketSupport(request);
        var ticketLiveIncident = repository.GetEventTicketLiveIncident(request);
        var ticketLogWork = repository.GetEventTicketLogWork(request);

        await Task.WhenAll(ticketLeague, ticketMatch, ticketOthers, ticketSupport, ticketLiveIncident, ticketLogWork);

        var eventTickets = new List<TicketInfo>();
        eventTickets.AddRange(ticketLeague.Result);
        eventTickets.AddRange(ticketMatch.Result);
        eventTickets.AddRange(ticketOthers.Result);
        eventTickets.AddRange(ticketSupport.Result);
        eventTickets.AddRange(ticketLiveIncident.Result);
        eventTickets.AddRange(ticketLogWork.Result);

        return eventTickets;
    }

    private async Task SetLastComment(IList<int> ticketIds, List<TicketInfo> ticketIsShowed)
    {
        var lastComments = (await repository.GetTwoLastComment(ticketIds)).ToList();

        foreach (var ticket in ticketIsShowed)
        {
            ticket.Messages = lastComments.Where(x => x.TicketId == ticket.TicketId).ToList();
        }
    }

    private static List<TicketInfo> SortEventTicketByCommentLatestDate(IEnumerable<TicketInfo> tickets)
    {
        return tickets
            .OrderByDescending(x => x.IsPinned)
            .ThenByDescending(x => x.LastCommentDate)
            .ToList();
    }
}