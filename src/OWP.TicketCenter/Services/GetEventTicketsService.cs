using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Services;

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

    //public async Task<IList<TicketBaseInfo>> GetAllTickets(QueryEventTicketsRequest request)
    //{
    //    var ticketIdSaved = await repository.GetTicketIdHaveSavedMessage();
    //    var eventTickets = await GetEventTickets(request);

    //    var ticketIsShowed = BuildTicketIsShowed(eventTickets, ticketIdSaved.ToHashSet());
    //    var ticketIds = ticketIsShowed.Select(x => x.TicketId).ToList();

    //    await SetLastComment(ticketIds, ticketIsShowed);

    //    return SortEventTicketByCommentLatestDate(ticketIsShowed);
    //}
    public async Task<IList<TicketBaseInfo>> GetAllTickets(QueryEventTicketsRequest request)
    {
        var eventTickets = await GetEventTickets(request);
        var ticketIds = eventTickets.Select(x => x.TicketId).ToList();

        await SetLastComment(ticketIds, eventTickets);

        return SortEventTicketByCommentLatestDate(eventTickets);
    }

    private async Task<List<TicketBaseInfo>> GetEventTickets(QueryEventTicketsRequest request)
    {
        var ticketLeague = repository.GetEventTicketLeague(request);
        var ticketMatch = repository.GetEventTicketMatch(request);
        var ticketOthers = repository.GetEventTicketOther(request);
        var ticketSupport = repository.GetEventTicketSupport(request);
        var ticketLiveIncident = repository.GetEventTicketLiveIncident(request);

        await Task.WhenAll(ticketLeague, ticketMatch, ticketOthers, ticketSupport, ticketLiveIncident);

        var eventTickets = new List<TicketBaseInfo>();
        eventTickets.AddRange(ticketLeague.Result);
        eventTickets.AddRange(ticketMatch.Result);
        eventTickets.AddRange(ticketOthers.Result);
        eventTickets.AddRange(ticketSupport.Result);
        eventTickets.AddRange(ticketLiveIncident.Result);

        return eventTickets;
    }

    private async Task SetLastComment(List<int> ticketIds, List<TicketBaseInfo> ticketIsShowed)
    {
        var lastComments = (await repository.GetTwoLastComment(ticketIds)).ToList();

        foreach (var ticket in ticketIsShowed)
        {
            ticket.Messages = lastComments.Where(x => x.TicketId == ticket.TicketId).ToList();
        }
    }

    private static List<TicketBaseInfo> SortEventTicketByCommentLatestDate(List<TicketBaseInfo> tickets)
    {
        return tickets
            .OrderByDescending(x => x.IsPinned)
            .ThenByDescending(x => x.LastCommentDate)
            .ToList();
    }

    private static List<TicketBaseInfo> BuildTicketIsShowed(List<TicketBaseInfo> allTicket, IEnumerable<int> ticketId)
    {
        var ticketSaved = allTicket.Where(t => ticketId.Contains(t.TicketId)).ToList();

        var ticketNotSaved = allTicket.Except(ticketSaved);

        var ticketIsShowed = ticketNotSaved.Where(x => DateTime.Now.Subtract(x.LastCommentDate).Days <= 30);

        ticketSaved.AddRange(ticketIsShowed);

        return ticketSaved;
    }
}