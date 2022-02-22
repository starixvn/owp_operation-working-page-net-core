using MediatR;
using OWP.TicketCenter.Middleware.Models;
using OWP.TicketCenter.Services;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Handlers
{
    public class TicketOtherDetailRequest : PermissionCheckedModel, IRequest<TicketOtherDetailViewModel>
    {
    }

    public class TicketOtherDetailHandler : IRequestHandler<TicketOtherDetailRequest, TicketOtherDetailViewModel>
    {
        private readonly TicketOtherDetailService service;

        public TicketOtherDetailHandler(TicketOtherDetailService service)
        {
            this.service = service;
        }

        public async Task<TicketOtherDetailViewModel> Handle(TicketOtherDetailRequest request, CancellationToken cancellationToken)
            => await service.GetTicketOtherDetail(request);
    }
}