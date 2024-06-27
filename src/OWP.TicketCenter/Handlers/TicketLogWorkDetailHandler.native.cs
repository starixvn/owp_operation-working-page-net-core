using MediatR;
using OWP.TicketCenter.Middleware.Models;
using OWP.TicketCenter.Services;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Handlers;

public class TicketLogWorkDetailRequest : PermissionCheckedModel, IRequest<TicketLogWorkDetailViewModel>;

public class TicketLogWorkDetailHandler : IRequestHandler<TicketLogWorkDetailRequest, TicketLogWorkDetailViewModel>
{
    private readonly TicketDetailService service;

    public TicketLogWorkDetailHandler(TicketDetailService service)
    {
        this.service = service;
    }

    public async Task<TicketLogWorkDetailViewModel> Handle(TicketLogWorkDetailRequest request, CancellationToken cancellationToken)
        => await service.GetTicketLogWorkDetail(request);
}