using MediatR;
using OWP.TicketCenter.Middleware.Models;
using OWP.TicketCenter.Services;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Handlers
{
    public class TicketLiveIncidentDetailRequest : PermissionCheckedModel, IRequest<TicketLiveIncidientDetailViewModel> { }

    public class TicketLiveIncidentDetailHandler : IRequestHandler<TicketLiveIncidentDetailRequest, TicketLiveIncidientDetailViewModel>
    {
        private readonly TicketDetailService service;

        public TicketLiveIncidentDetailHandler(TicketDetailService service)
        {
            this.service = service;
        }

        public async Task<TicketLiveIncidientDetailViewModel> Handle(TicketLiveIncidentDetailRequest request, CancellationToken cancellationToken)
            => await service.GetTicketLiveIncidentDetail(request);
    }
}