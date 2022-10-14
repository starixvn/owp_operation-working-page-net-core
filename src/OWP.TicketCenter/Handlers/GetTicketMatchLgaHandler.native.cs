using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Handlers
{
    public class GetTicketMatchLgaRequest : IRequest<IEnumerable<HasTicketModel>>
    {
        public string OwpMatchIds { get; set; }
    }
    public class GetTicketMatchLgaHandler : IRequestHandler<GetTicketMatchLgaRequest, IEnumerable<HasTicketModel>>
    {
        private readonly Repository repository;

        public GetTicketMatchLgaHandler(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<HasTicketModel>> Handle(GetTicketMatchLgaRequest request, CancellationToken cancellationToken)
          => (await repository.GetTicketMatchLga(request.OwpMatchIds));
    }
}