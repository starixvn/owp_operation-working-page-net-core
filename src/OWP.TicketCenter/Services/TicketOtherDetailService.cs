

using AutoMapper;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Enum;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Services
{
    public interface TicketOtherDetailService
    {
        Task<TicketOtherDetailViewModel> GetTicketOtherDetail(TicketOtherDetailRequest request);
    }

    public class TicketOtherDetailServiceImp : TicketOtherDetailService
    {
        private readonly Repository repository;

        private readonly TicketDetailService ticketDetailService;

        private readonly IMapper mapper;

        public TicketOtherDetailServiceImp(Repository repository, TicketDetailService ticketDetailService, IMapper mapper)
        {
            this.repository = repository;
            this.ticketDetailService = ticketDetailService;
            this.mapper = mapper;
        }

        public async Task<TicketOtherDetailViewModel> GetTicketOtherDetail(TicketOtherDetailRequest request)
        {
            var ticketId = request.TicketId;
            var actorId = request.ActorId;

            var ticketOther = await repository.GetTicketOtherDetail(ticketId);
            var ticketFirst = ticketOther.Any()
                ? ticketOther.First()
                : new EventTicketOtherModel();

            var viewModel
                = mapper.Map<TicketOtherDetailViewModel>(ticketFirst);
            var messages = ticketDetailService.BuildMessage(ticketOther.ToList());

            var criteria = new UpdateSingleRecipientReaderCriteria
            {
                TicketId=ticketId,
                IsRead=true,
                RecipientId=actorId
            };

            await repository.UpdateSingleRecipientReader(criteria);

            var recipients = await repository.GetRecipientByTicketId(ticketId);

            var issueCreator = await repository.GetIssueCreator(ticketId);

            viewModel.CreatorInfo = issueCreator;
            viewModel.Recipients = recipients?.ToList();
            viewModel.Messages = messages?.ToList();
            viewModel.EventType = (int)EventType.Other;

            return viewModel;
        }
    }
}