
using AutoMapper;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Enum;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Services
{
    public interface TicketSupportDetailService
    {
        Task<TicketSupportDetailViewModel> GetTicketSupportDetail(TicketSupportDetailRequest request);
    }
    public class TicketSupportDetailServiceImp : TicketSupportDetailService
    {
        private readonly Repository repository;

        private readonly TicketDetailService ticketDetailService;

        private readonly IMapper mapper;
        public TicketSupportDetailServiceImp(Repository repository, TicketDetailService ticketDetailService, IMapper mapper)
        {
            this.repository = repository;
            this.ticketDetailService = ticketDetailService;
            this.mapper = mapper;
        }

        public async Task<TicketSupportDetailViewModel> GetTicketSupportDetail(TicketSupportDetailRequest request)
        {
            var ticketId = request.TicketId;
            var actorId = request.ActorId;

            var ticketSupport = await repository.GetTicketSupportDetail(ticketId);
            var ticketFirst = ticketSupport.Any()
                ? ticketSupport.First()
                : new EventTicketSupportModel();

            var viewModel
                = mapper.Map<TicketSupportDetailViewModel>(ticketFirst);
            var messages = ticketDetailService.BuildMessage(ticketSupport.ToList());

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
            viewModel.EventType = (int)EventType.Support;

            return viewModel;
        }
    }
}
