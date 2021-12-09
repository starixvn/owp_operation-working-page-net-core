using System.Threading;
using System.Threading.Tasks;
using Fanex.NAP.Client.Helpers.Web;
using MediatR;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Middleware.Models;
using OWP.TicketCenter.Notification.Services;

namespace OWP.TicketCenter.Handlers
{
    public class UpdateTicketStatusRequest : PermissionCheckedModel, IRequest<string>
    {
        public string TicketStatus { get; set; }
    }

    public class UpdateTicketStatusHandler : IRequestHandler<UpdateTicketStatusRequest, string>
    {
        private readonly Repository repository;

        private readonly INapSession session;

        private readonly NotificationService notificationService;

        public UpdateTicketStatusHandler(
            Repository repository,
            INapSession session,
            NotificationService notificationService)
        {
            this.repository = repository;
            this.session = session;
            this.notificationService = notificationService;
        }

        public async Task<string> Handle(UpdateTicketStatusRequest request, CancellationToken cancellationToken)
        {
            var criteria = new UpdateTicketStatusCriteria
            {
                TicketId = request.TicketId,
                TicketStatus = request.TicketStatus,
                ModifiedBy = session.UserId
            };

            await repository.UpdateTicketStatus(criteria);

            await notificationService.NotifyChangeStatusIncome(request.TicketId);

            return string.Empty;
        }
    }
}