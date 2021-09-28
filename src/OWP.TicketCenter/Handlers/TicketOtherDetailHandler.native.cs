using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OWP.TicketCenter.Services;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Handlers
{
    public class TicketOtherDetailRequest : IRequest<TicketOtherDetailViewModel>
    {
        public int TicketId { get; set; }
    }

    public class TicketOtherDetailHandler : IRequestHandler<TicketOtherDetailRequest, TicketOtherDetailViewModel>
    {
        private readonly TicketOtherDetailService service;

        public TicketOtherDetailHandler(TicketOtherDetailService service)
        {
            this.service = service;
        }

        public async Task<TicketOtherDetailViewModel> Handle(TicketOtherDetailRequest request, CancellationToken cancellationToken)
            => await service.GetTicketOtherDetail(request.TicketId);
    }
}