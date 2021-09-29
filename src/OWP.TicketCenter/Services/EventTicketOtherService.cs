using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using OWP.TicketCenter.Builder;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Services
{
    public interface EventTicketOtherService
    {
        Task<IList<TicketOtherDetailViewModel>> GetAllTicketOther(QueryEventTicketsRequest request);
    }

    public class EventTicketOtherServiceImp : EventTicketOtherService
    {
        private readonly Repository repository;

        private readonly IMapper mapper;

        public EventTicketOtherServiceImp(Repository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }


        public async Task<IList<TicketOtherDetailViewModel>> GetAllTicketOther(QueryEventTicketsRequest request)
        {
            var ticketOthers = await repository.GetEventTicketOther(request);

            var ticketOthersDic = ticketOthers
                .GroupBy(t => t.TicketId)
                .ToDictionary(t => t.Key, v => v.ToList());

            var ticketOthersViewModel = new List<TicketOtherDetailViewModel>();
            foreach (var key in ticketOthersDic.Keys)
            {
                var ticketOtherFirst = ticketOthersDic[key].First();
                var viewModel = mapper.Map<TicketOtherDetailViewModel>(ticketOtherFirst);

                var messageModel = BuildMessageModels(ticketOthersDic[key]);
                var messageDetail = TwoLastCommentBuilder.GetTwoLastComment(messageModel.ToList());

                viewModel.Messages = messageDetail;
                ticketOthersViewModel.Add(viewModel);
            }
            return ticketOthersViewModel;
        }

        private IEnumerable<MessageModel> BuildMessageModels(IEnumerable<EventTicketOtherModel> models)
        => models
            .Select(x => mapper.Map<MessageModel>(x))
            .ToList();
    }
}
