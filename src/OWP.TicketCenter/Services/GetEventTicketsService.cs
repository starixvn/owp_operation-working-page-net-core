using AutoMapper;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Services
{
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
            var eventTickets = new List<TicketBaseInfo>();
            eventTickets.AddRange(ticketLeague);
            eventTickets.AddRange(ticketMatch);
            eventTickets.AddRange(ticketOthers);
            eventTickets.AddRange(ticketSupport);
            var ticketIds = eventTickets.Select(x => x.TicketId).ToList();
            var lastComments = await repository.GetTwoLastCommnet(ticketIds);

            foreach (var ticket in eventTickets)
            {
                ticket.Messages = lastComments.Where(x => x.TicketId == ticket.TicketId).ToList();
            }

            return SortEventTicketByCommentLatestDate(eventTickets);
        }

        private List<TicketBaseInfo> SortEventTicketByCommentLatestDate(
           List<TicketBaseInfo> tickets)
           => tickets.OrderByDescending(x => x.IsPinned).ThenByDescending(x => x.LastCommentDate).ToList();
    }
}