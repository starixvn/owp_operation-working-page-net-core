using MediatR;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Middleware.Models;

namespace OWP.TicketCenter.Handlers;

public class UpdateTicketSubTypeRequest : PermissionCheckedModel, IRequest<Unit>
{
    public int TicketSubTypeId { get; set; }
}

public class UpdateTicketSubTypeHandler : IRequestHandler<UpdateTicketSubTypeRequest, Unit>
{
    private readonly Repository repository;

    public UpdateTicketSubTypeHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<Unit> Handle(UpdateTicketSubTypeRequest request, CancellationToken cancellationToken)
    {
        await repository.UpdateTicketSubType((new UpdateTicketSubTypeCriteria()
        {
            TicketId = request.TicketId,
            TicketSubTypeId = request.TicketSubTypeId
        }));

        return Unit.Value;
    }
}