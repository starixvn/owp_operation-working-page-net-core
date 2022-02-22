using MediatR;
using OWP.TicketCenter.Middleware.Models;
using OWP.TicketCenter.Services;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Handlers
{
    public class TicketSupportDetailRequest : PermissionCheckedModel, IRequest<TicketSupportDetailViewModel>
    {
    }

    public class TicketSupportDetailHandler : IRequestHandler<TicketSupportDetailRequest, TicketSupportDetailViewModel>
    {
        private readonly TicketSupportDetailService service;

        public TicketSupportDetailHandler(TicketSupportDetailService service)
        {
            this.service = service;
        }

        public async Task<TicketSupportDetailViewModel> Handle(TicketSupportDetailRequest request, CancellationToken cancellationToken)
            => await service.GetTicketSupportDetail(request);
    }
}