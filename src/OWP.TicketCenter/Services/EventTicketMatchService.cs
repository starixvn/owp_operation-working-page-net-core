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
    public interface EventTicketMatchService
    {
        Task<IList<TicketMatchDetailViewModel>> GetAllMatch(QueryEventTicketsRequest request);
    }

    public class EventTicketMatchServiceImp : EventTicketMatchService
    {
        private readonly Repository repository;

        private readonly IMapper mapper;

        public EventTicketMatchServiceImp(Repository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }


        public async Task<IList<TicketMatchDetailViewModel>> GetAllMatch(QueryEventTicketsRequest request)
        {
            var ticketMatch = await repository.GetEventTicketMatch(request);

            var matchDic = ticketMatch
                .GroupBy(t => t.TicketId)
                .ToDictionary(t => t.Key, v => v.ToList());

            var matchViewModel = new List<TicketMatchDetailViewModel>();
            foreach (var key in matchDic.Keys)
            {
                var matchFirst = matchDic[key].First();
                var viewModel = mapper.Map<TicketMatchDetailViewModel>(matchFirst);

                var messageModel = BuildMessageModels(matchDic[key]);
                var messageDetail = TwoLastCommentBuilder.GetTwoLastComment(messageModel.ToList());

                viewModel.Messages = messageDetail;
                matchViewModel.Add(viewModel);
            }
            return matchViewModel;
        }

        private static IEnumerable<MessageModel> BuildMessageModels(IEnumerable<EventTicketMatchModel> models)
            => models.Select(x => new MessageModel
            {
                UserId = x.UserId,
                Username = x.Username,
                CreatedDate = x.CreatedDate,
                Message = x.Message,
                MessageId = x.MessageId
            }).ToList();

    }
}
