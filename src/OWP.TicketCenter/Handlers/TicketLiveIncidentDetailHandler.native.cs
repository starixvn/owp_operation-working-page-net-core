namespace OWP.TicketCenter.Handlers;

using MediatR;
using Middleware.Models;
using Services;
using ViewModels;

public class TicketLiveIncidentDetailRequest : PermissionCheckedModel, IRequest<TicketLiveIncidentDetailViewModel> { }

public class TicketLiveIncidentDetailHandler : IRequestHandler<TicketLiveIncidentDetailRequest, TicketLiveIncidentDetailViewModel>
{
    private readonly TicketDetailService service;

    public TicketLiveIncidentDetailHandler(TicketDetailService service)
    {
        this.service = service;
    }

    public async Task<TicketLiveIncidentDetailViewModel> Handle(TicketLiveIncidentDetailRequest request, CancellationToken cancellationToken)
        => await service.GetTicketLiveIncidentDetail(request);
}