using OWP.TicketCenter.Criteria.NewTicket;
using OWP.TicketCenter.Handlers;
using AutoMapper;

namespace OWP.TicketCenter
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<NewTicketIssueEventRequest,AddNewTicketIssueEventCriteria>();
            
        }
    }
}