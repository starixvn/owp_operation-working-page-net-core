using MediatR;
using OWP.TicketCenter.Middleware.Models;
using OWP.TicketCenter.Services;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Handlers
{
    public class TicketLiveIncidentDetailRequest : PermissionCheckedModel, IRequest<TicketMatchDetailViewModel> { }

    public class TicketLiveIncidentDetailHandler : IRequestHandler<TicketLiveIncidentDetailRequest, TicketMatchDetailViewModel>
    {
        private readonly TicketLiveIncidentDetailService service;

        public TicketLiveIncidentDetailHandler(TicketLiveIncidentDetailService service)
        {
            this.service = service;
        }

        public async Task<TicketMatchDetailViewModel> Handle(TicketLiveIncidentDetailRequest request, CancellationToken cancellationToken)
            => await service.GetTicketMatchDetail(request);
    }
}