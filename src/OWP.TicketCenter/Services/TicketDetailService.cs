using AutoMapper;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Middleware.Models;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Services
{
    public interface TicketDetailService
    {
        Task<TicketMatchDetailViewModel> GetTicketMatchDetail(TicketMatchDetailRequest request);
        Task<TicketLiveIncidentDetailViewModel> GetTicketLiveIncidentDetail(TicketLiveIncidentDetailRequest request);
        Task<TicketLeagueDetailViewModel> GetTicketLeagueDetail(TicketLeagueDetailRequest request);
        Task<TicketOtherDetailViewModel> GetTicketOtherDetail(TicketOtherDetailRequest request);
        Task<TicketSupportDetailViewModel> GetTicketSupportDetail(TicketSupportDetailRequest request);
    }

    public class TicketDetailServiceImp : TicketDetailService
    {
        private readonly IMapper mapper;
        private readonly Repository repository;
        private readonly MessageService messageService;

        public TicketDetailServiceImp(IMapper mapper, Repository repository, MessageService messageService)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.messageService = messageService;
        }

        public async Task<TicketMatchDetailViewModel> GetTicketMatchDetail(TicketMatchDetailRequest request)
        {
            var ticketMatch = await repository.GetTicketMatchDetail(request.TicketId);
            var viewModel = mapper.Map<TicketMatchDetailViewModel>(ticketMatch);

            await BuildExtInfoTicketDetail(viewModel, request);

            return viewModel;
        }

        public async Task<TicketLiveIncidentDetailViewModel> GetTicketLiveIncidentDetail(TicketLiveIncidentDetailRequest request)
        {
            var ticketMatch = await repository.GetTicketLiveIncidentDetail(request.TicketId);
            var viewModel = mapper.Map<TicketLiveIncidentDetailViewModel>(ticketMatch);

            await BuildExtInfoTicketDetail(viewModel, request);

            return viewModel;
        }

        public async Task<TicketLeagueDetailViewModel> GetTicketLeagueDetail(TicketLeagueDetailRequest request)
        {
            var ticketLeague = await repository.GetTicketLeagueDetail(request.TicketId);
            var viewModel = mapper.Map<TicketLeagueDetailViewModel>(ticketLeague);

            await BuildExtInfoTicketDetail(viewModel, request);

            return viewModel;
        }

        public async Task<TicketOtherDetailViewModel> GetTicketOtherDetail(TicketOtherDetailRequest request)
        {
            var ticketOther = await repository.GetTicketOtherDetail(request.TicketId);
            var viewModel = mapper.Map<TicketOtherDetailViewModel>(ticketOther);

            await BuildExtInfoTicketDetail(viewModel, request);

            return viewModel;
        }

        public async Task<TicketSupportDetailViewModel> GetTicketSupportDetail(TicketSupportDetailRequest request)
        {
            var ticketSupport = await repository.GetTicketSupportDetail(request.TicketId);
            var viewModel = mapper.Map<TicketSupportDetailViewModel>(ticketSupport);

            await BuildExtInfoTicketDetail(viewModel, request);

            return viewModel;
        }

        public async Task BuildExtInfoTicketDetail(TicketDetailBaseViewModel model, PermissionCheckedModel permissionModel)
        {
            var ticketId = permissionModel.TicketId;

            if (model == null)
            {
                return;
            };

            (var canShowMoreMessage, var messages) = !permissionModel.FirstMessageId.HasValue
            ? await messageService.GetMessageListInLast2Days(ticketId)
            : await messageService.GetMessageListByGreaterMessageId(ticketId, permissionModel.FirstMessageId.Value);
            var criteria = new UpdateSingleRecipientReaderCriteria
            {
                TicketId = ticketId,
                IsRead = true,
                RecipientId = permissionModel.ActorId
            };
            await repository.UpdateSingleRecipientReader(criteria);

            var recipients = await repository.GetRecipientByTicketId(ticketId);
            var issueCreator = await repository.GetIssueCreator(ticketId);

            model.CreatorInfo = issueCreator;
            model.Recipients = recipients?.ToList();
            model.Messages = messages?.ToList();
            model.CanShowMoreMessage = canShowMoreMessage;
        }
    }
}