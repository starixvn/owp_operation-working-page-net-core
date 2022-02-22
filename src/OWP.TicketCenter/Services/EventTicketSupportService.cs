using AutoMapper;
using OWP.TicketCenter.Builder;
using OWP.TicketCenter.Enum;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Services
{
    public interface EventTicketSupportService
    {
        Task<IList<TicketSupportDetailViewModel>> GetAllTicketSupport(QueryEventTicketsRequest request);
    }

    public class EventTicketSupportServiceImp : EventTicketSupportService
    {
        private readonly Repository repository;

        private readonly IMapper mapper;

        public EventTicketSupportServiceImp(Repository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IList<TicketSupportDetailViewModel>> GetAllTicketSupport(QueryEventTicketsRequest request)
        {
            var ticketSupport = await repository.GetEventTicketSupport(request);
            var ticketSupportDic = ticketSupport
                .GroupBy(t => t.TicketId)
                .ToDictionary(t => t.Key, v => v.ToList());

            var ticketSupportViewModel = new List<TicketSupportDetailViewModel>();
            foreach (var key in ticketSupportDic.Keys)
            {
                var ticketOtherFirst = ticketSupportDic[key].First();
                var viewModel = mapper.Map<TicketSupportDetailViewModel>(ticketOtherFirst);

                var messageModel = BuildMessageModels(ticketSupportDic[key]);
                var messageDetail = TwoLastCommentBuilder.GetTwoLastComment(messageModel.ToList());

                viewModel.Messages = messageDetail;
                viewModel.IsRead = ticketSupportDic[key].All(l => l.IsRead);
                viewModel.EventType = (int)EventType.Support;

                ticketSupportViewModel.Add(viewModel);
            }
            return ticketSupportViewModel;
        }

        private IEnumerable<MessageResponseModel> BuildMessageModels(IEnumerable<EventTicketSupportModel> models)
            => models
                .Select(x => mapper.Map<MessageResponseModel>(x))
                .ToList();
    }
}