using AutoMapper;
using OWP.Extensions;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter
{
    using Models;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<NewTicketIssueLgaRequest, AddNewTicketIssueLgaCriteria>();
            CreateMap<NewTicketIssueEventRequest, AddNewTicketIssueEventCriteria>();
            CreateMap<NewTicketIssueOtherRequest, AddNewTicketIssueOtherCriteria>();
            CreateMap<NewTicketIssueLeagueRequest, AddNewTicketIssueLeagueCriteria>();
            CreateMap<NewTicketIssueSupportRequest, AddNewTicketIssueSupportCriteria>();

            CreateMap<NewTicketIssueLgaRequest, NewMessageRequestModel>().ForMember(
                m => m.FileId,
                opt => opt.Ignore());
            CreateMap<NewTicketIssueEventRequest, NewMessageRequestModel>().ForMember(
                m => m.FileId,
                opt => opt.Ignore());
            CreateMap<NewTicketIssueOtherRequest, NewMessageRequestModel>().ForMember(
                m => m.FileId,
                opt => opt.Ignore());
            CreateMap<NewTicketIssueSupportRequest, NewMessageRequestModel>().ForMember(
                m => m.FileId,
                opt => opt.Ignore());
            CreateMap<NewTicketIssueLeagueRequest, NewMessageRequestModel>().ForMember(
                m => m.FileId,
                opt => opt.Ignore());
            CreateMap<QueryEventTicketsRequest, GetEventTicketLeagueCriteria>();
            CreateMap<QueryEventTicketsRequest, GetEventTicketMatchCriteria>();
            CreateMap<QueryEventTicketsRequest, GetEventTicketOtherCriteria>();
            CreateMap<QueryEventTicketsRequest, GetEventTicketSupportCriteria>();

            CreateMap<EventTicketLeagueModel, TicketLeagueDetailViewModel>()
                .ForMember(
                m => m.Messages,
                opt => opt.Ignore())
                .ForMember(
                m => m.Recipients, opt => opt.Ignore())
                .ForMember(
                m => m.CommentLatestDate, opt => opt.Ignore());
            CreateMap<EventTicketMatchModel, TicketMatchDetailViewModel>()
                .ForMember(
                m => m.Messages,
                opt => opt.Ignore())
                .ForMember(
                    m => m.Recipients, opt => opt.Ignore())
                .ForMember(
                    m => m.CommentLatestDate, opt => opt.Ignore());
            CreateMap<EventTicketOtherModel, TicketOtherDetailViewModel>()
                .ForMember(
                m => m.Messages,
                opt => opt.Ignore())
                .ForMember(
                    m => m.Recipients, opt => opt.Ignore())
                .ForMember(
                    m => m.CommentLatestDate, opt => opt.Ignore());
            CreateMap<EventTicketSupportModel, TicketSupportDetailViewModel>()
                .ForMember(
                    m => m.Messages,
                    opt => opt.Ignore())
                .ForMember(
                    m => m.Recipients, opt => opt.Ignore())
                .ForMember(
                    m => m.CommentLatestDate, opt => opt.Ignore());

            CreateMap<EventTicketLeagueModel, MessageDetailViewModel>();
            CreateMap<EventTicketMatchModel, MessageDetailViewModel>();
            CreateMap<EventTicketOtherModel, MessageDetailViewModel>();
            CreateMap<EventTicketSupportModel, MessageDetailViewModel>();

            CreateMap<EventTicketLeagueModel, MessageResponseModel>();
            CreateMap<EventTicketMatchModel, MessageResponseModel>();
            CreateMap<EventTicketOtherModel, MessageResponseModel>();
            CreateMap<EventTicketSupportModel, MessageResponseModel>();

            CreateMap<InsertSingleRecipientRequest, AddSingleRecipientCriteria>();
            CreateMap<RemoveSingleRecipientRequest, RemoveSingleRecipientCriteria>();
        }
    }
}