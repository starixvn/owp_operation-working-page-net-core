using AutoMapper;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Criteria.NewTicket;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.ViewModels;

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
            CreateMap<EventTicketLeagueModel, TicketLeagueDetailViewModel>().ForMember(
                m => m.Messages,
                opt => opt.Ignore());
            CreateMap<EventTicketMatchModel, TicketMatchDetailViewModel>().ForMember(
                m => m.Messages,
                opt => opt.Ignore()); ;
            CreateMap<EventTicketOtherModel, TicketOtherDetailViewModel>().ForMember(
                m => m.Messages,
                opt => opt.Ignore()); ;
        }
    }
}