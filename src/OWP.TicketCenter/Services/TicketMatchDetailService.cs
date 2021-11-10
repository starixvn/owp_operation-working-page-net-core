using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Enum;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Services
{
    public interface TicketMatchDetailService
    {
        Task<TicketMatchDetailViewModel> GetTicketMatchDetail(TicketMatchDetailRequest request);
    }

    public class TicketMatchDetailServiceImp : TicketMatchDetailService
    {
        private readonly Repository repository;

        private readonly TicketDetailService ticketDetailService;

        private readonly IMapper mapper;

        public TicketMatchDetailServiceImp(Repository repository, TicketDetailService ticketDetailService, IMapper mapper)
        {
            this.repository = repository;
            this.ticketDetailService = ticketDetailService;
            this.mapper = mapper;
        }

        public async Task<TicketMatchDetailViewModel> GetTicketMatchDetail(TicketMatchDetailRequest request)
        {
            var ticketId = request.TicketId;
            var actorId = request.ActorId;

            var ticketMatch = await repository.GetTicketMatchDetail(ticketId);
            var ticketFirst = ticketMatch.Any()
                ? ticketMatch.First()
            : new EventTicketMatchModel();

            var viewModel = mapper.Map<TicketMatchDetailViewModel>(ticketFirst);
            var messages = ticketDetailService.BuildMessage(ticketMatch?.ToList());

            var criteria = new UpdateSingleRecipientReaderCriteria
            {
                TicketId = ticketId,
                IsRead = true,
                RecipientId = actorId
            };
            await repository.UpdateSingleRecipientReader(criteria);

            var recipients = await repository.GetRecipientByTicketId(ticketId);
            var issueCreator = await repository.GetIssueCreator(ticketId);

            viewModel.CreatorInfo = issueCreator;
            viewModel.Recipients = recipients?.ToList();
            viewModel.Messages = messages?.ToList();
            viewModel.EventType = (int)EventType.Match;

            return viewModel;
        }
    }
}
