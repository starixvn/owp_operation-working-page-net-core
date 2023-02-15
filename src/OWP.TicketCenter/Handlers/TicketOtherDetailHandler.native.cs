namespace OWP.TicketCenter.Handlers;

using MediatR;
using Middleware.Models;
using Services;
using ViewModels;

public class TicketOtherDetailRequest : PermissionCheckedModel, IRequest<TicketOtherDetailViewModel> { }

public class TicketOtherDetailHandler : IRequestHandler<TicketOtherDetailRequest, TicketOtherDetailViewModel>
{
    private readonly TicketDetailService service;

    public TicketOtherDetailHandler(TicketDetailService service)
    {
        this.service = service;
    }

    public async Task<TicketOtherDetailViewModel> Handle(TicketOtherDetailRequest request, CancellationToken cancellationToken)
        => await service.GetTicketOtherDetail(request);
}