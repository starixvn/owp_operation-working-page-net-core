using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OWP.TicketCenter.Middleware.Models;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Services;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Handlers
{
    public class TicketMatchDetailRequest : PermissionCheckedModel, IRequest<TicketMatchDetailViewModel>
    {
      
    }

    public class TicketMatchDetailHandler: IRequestHandler<TicketMatchDetailRequest, TicketMatchDetailViewModel>
    {
        private readonly TicketMatchDetailService service;

        public TicketMatchDetailHandler(TicketMatchDetailService service)
        {
            this.service = service;
        }

        public async Task<TicketMatchDetailViewModel> Handle(TicketMatchDetailRequest request,
            CancellationToken cancellationToken)
            => await service.GetTicketMatchDetail(request);
    }
}
