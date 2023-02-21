using MediatR;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Middleware.Models;
using OWP.TicketCenter.Notification.Services;

namespace OWP.TicketCenter.Handlers;

public class UpdateTicketSubTypeRequest : PermissionCheckedModel, IRequest<Unit>
{
    public int TicketSubTypeId { get; set; }
}

public class UpdateTicketSubTypeHandler : IRequestHandler<UpdateTicketSubTypeRequest, Unit>
{
    private readonly Repository repository;
    private readonly NotificationService notificationService;

    public UpdateTicketSubTypeHandler(Repository repository, NotificationService notificationService)
    {
        this.repository = repository;
        this.notificationService = notificationService;
    }

    public async Task<Unit> Handle(UpdateTicketSubTypeRequest request, CancellationToken cancellationToken)
    {
        await repository.UpdateTicketSubType(new UpdateTicketSubTypeCriteria
        {
            TicketId = request.TicketId,
            TicketSubTypeId = request.TicketSubTypeId
        });

        await notificationService.NotifyChangeSubTypeIncome(request.TicketId);

        return Unit.Value;
    }
}