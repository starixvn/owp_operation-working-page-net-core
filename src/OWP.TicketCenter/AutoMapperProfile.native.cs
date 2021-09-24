using OWP.TicketCenter.Criteria.NewTicket;
using OWP.TicketCenter.Handlers;
using AutoMapper;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<NewTicketIssueEventRequest,AddNewTicketIssueEventCriteria>();
            CreateMap<NewTicketIssueOtherRequest, AddNewTicketIssueOtherCriteria>();
            CreateMap<NewTicketIssueLeagueRequest, AddNewTicketIssueLeagueCriteria>();
        }
    }
}