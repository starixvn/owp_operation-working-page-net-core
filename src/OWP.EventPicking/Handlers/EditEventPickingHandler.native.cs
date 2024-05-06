
namespace OWP.EventPicking.Handlers;

public class EditEventPickingRequest : EventPickingRequest, IRequest<EventPickingResult>
{ }

public class EditEventPickingHandler : IRequestHandler<EditEventPickingRequest, EventPickingResult>
{
    private readonly QueryEventPickingService service;

    public EditEventPickingHandler(QueryEventPickingService service)
    {
        this.service = service;
    }

    public async Task<EventPickingResult> Handle(EditEventPickingRequest request, CancellationToken cancellationToken)
    {
        // TODO
        return await service.Execute(request);
    }


}