using MediatR;
using OWP.TicketCenter.Middleware.Models;
using OWP.TicketCenter.Services;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Handlers
{
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
}