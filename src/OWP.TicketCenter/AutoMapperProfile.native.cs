using AutoMapper;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Criteria.NewTicket;
using OWP.TicketCenter.Handlers;

namespace OWP.TicketCenter
{
    using Models;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<NewTicketIssueEventRequest, AddNewTicketIssueEventCriteria>();
            CreateMap<NewTicketIssueOtherRequest, AddNewTicketIssueOtherCriteria>();
            CreateMap<NewTicketIssueLeagueRequest, AddNewTicketIssueLeagueCriteria>();
            CreateMap<QueryEventTicketsRequest, GetEventTicketLeagueCriteria>();
            CreateMap<QueryEventTicketsRequest, GetEventTicketMatchCriteria>();
            CreateMap<QueryEventTicketsRequest, GetEventTicketOtherCriteria>();
            CreateMap<EventTicketLeagueRaw, EventTicketLeague>();
            CreateMap<EventTicketMatchRaw, EventTicketMatch>();
            CreateMap<EventTicketOtherRaw, EventTicketOther>();
        }
    }
}