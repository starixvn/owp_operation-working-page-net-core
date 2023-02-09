namespace OWP.TicketCenter.Handlers;

using MediatR;
using Middleware.Models;
using Services;
using ViewModels;

public class TicketSupportDetailRequest : PermissionCheckedModel, IRequest<TicketSupportDetailViewModel> { }

public class TicketSupportDetailHandler : IRequestHandler<TicketSupportDetailRequest, TicketSupportDetailViewModel>
{
    private readonly TicketDetailService service;

    public TicketSupportDetailHandler(TicketDetailService service)
    {
        this.service = service;
    }

    public async Task<TicketSupportDetailViewModel> Handle(TicketSupportDetailRequest request, CancellationToken cancellationToken)
        => await service.GetTicketSupportDetail(request);
}