using MediatR;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Middleware.Models;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Services;

namespace OWP.TicketCenter.Handlers;

public class UpdateTicketSubTypeRequest : PermissionCheckedModel, IRequest<Unit>
{
    public TicketInfo TicketInfo { get; set; }
}

public class UpdateTicketSubTypeHandler : IRequestHandler<UpdateTicketSubTypeRequest, Unit>
{
    private readonly NotificationService notificationService;
    private readonly Repository repository;

    public UpdateTicketSubTypeHandler(Repository repository, NotificationService notificationService)
    {
        this.repository = repository;
        this.notificationService = notificationService;
    }

    public async Task<Unit> Handle(UpdateTicketSubTypeRequest request, CancellationToken cancellationToken)
    {
        var ticketSubTypeId = request.TicketInfo.TicketSubTypeId ?? 0;
        await repository.UpdateTicketSubType(new UpdateTicketSubTypeCriteria
        {
            TicketId = request.TicketInfo.TicketId,
            TicketSubTypeId = ticketSubTypeId
        });

        await notificationService.NotifyChangeSubTypeIncome(request.TicketInfo);

        return Unit.Value;
    }
}