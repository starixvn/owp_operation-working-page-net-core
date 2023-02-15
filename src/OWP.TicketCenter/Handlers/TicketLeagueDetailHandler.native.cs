namespace OWP.TicketCenter.Handlers;

using MediatR;
using Middleware.Models;
using Services;
using ViewModels;

public class TicketLeagueDetailRequest : PermissionCheckedModel, IRequest<TicketLeagueDetailViewModel> { }

public class TicketLeagueDetailHandler : IRequestHandler<TicketLeagueDetailRequest, TicketLeagueDetailViewModel>
{
    private readonly TicketDetailService service;

    public TicketLeagueDetailHandler(TicketDetailService service)
    {
        this.service = service;
    }

    public async Task<TicketLeagueDetailViewModel> Handle(TicketLeagueDetailRequest request, CancellationToken cancellationToken)
        => await service.GetTicketLeagueDetail(request);
}