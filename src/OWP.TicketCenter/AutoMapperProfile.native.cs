using OWP.TicketCenter.Criteria.NewTicket;
using OWP.TicketCenter.Handlers;
using AutoMapper;
using OWP.TicketCenter.Criteria;

namespace OWP.TicketCenter
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<NewTicketIssueEventRequest,AddNewTicketIssueEventCriteria>();
            CreateMap<NewTicketIssueOtherRequest, AddNewTicketIssueOtherCriteria>();
            CreateMap<NewTicketIssueLeagueRequest, AddNewTicketIssueLeagueCriteria>();
            CreateMap<QueryEventTicketsRequest, GetEventTicketLeagueCriteria>();
            CreateMap<QueryEventTicketsRequest, GetEventTicketMatchCriteria>();
            CreateMap<QueryEventTicketsRequest, GetEventTicketOtherCriteria>();

        }
    }
}