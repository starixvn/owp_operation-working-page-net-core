namespace OWP.TicketCenter.Handlers
{
    using MediatR;
    using OWP.TicketCenter.Criteria;
    using OWP.TicketCenter.Middleware.Models;
    using OWP.TicketCenter.Notification.Services;

    public class UpdateTicketStatusRequest : PermissionCheckedModel, IRequest<string>
    {
        public string TicketStatus { get; set; }
    }

    public class UpdateTicketStatusHandler : IRequestHandler<UpdateTicketStatusRequest, string>
    {
        private readonly Repository repository;

        private readonly NotificationService notificationService;

        public UpdateTicketStatusHandler(
            Repository repository,
            NotificationService notificationService)
        {
            this.repository = repository;
            this.notificationService = notificationService;
        }

        public async Task<string> Handle(UpdateTicketStatusRequest request, CancellationToken cancellationToken)
        {
            var criteria = new UpdateTicketStatusCriteria
            {
                TicketId = request.TicketId,
                TicketStatus = request.TicketStatus,
                ModifiedBy = request.ActorId
            };

            await repository.UpdateTicketStatus(criteria);

            await notificationService.NotifyChangeStatusIncome(request.TicketId);

            return string.Empty;
        }
    }
}