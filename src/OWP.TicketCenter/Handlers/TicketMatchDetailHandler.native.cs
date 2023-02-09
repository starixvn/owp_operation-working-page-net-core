namespace OWP.TicketCenter.Handlers;

using MediatR;
using Middleware.Models;
using Services;
using ViewModels;

public class TicketMatchDetailRequest : PermissionCheckedModel, IRequest<TicketMatchDetailViewModel> { }

public class TicketMatchDetailHandler : IRequestHandler<TicketMatchDetailRequest, TicketMatchDetailViewModel>
{
    private readonly TicketDetailService service;

    public TicketMatchDetailHandler(TicketDetailService service)
    {
        this.service = service;
    }

    public async Task<TicketMatchDetailViewModel> Handle(TicketMatchDetailRequest request, CancellationToken cancellationToken)
        => await service.GetTicketMatchDetail(request);
}