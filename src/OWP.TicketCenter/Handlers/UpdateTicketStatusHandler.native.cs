using MediatR;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Middleware.Models;
using OWP.TicketCenter.Notification.Services;

namespace OWP.TicketCenter.Handlers;

public class UpdateTicketStatusRequest : PermissionCheckedModel, IRequest<string>
{
    public string TicketStatus { get; set; }
}

public class UpdateTicketStatusHandler : IRequestHandler<UpdateTicketStatusRequest, string>
{
    private readonly NotificationService notificationService;
    private readonly Repository repository;

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