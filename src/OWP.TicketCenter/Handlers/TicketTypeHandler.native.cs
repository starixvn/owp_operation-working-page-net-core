using MediatR;
using OWP.TicketCenter.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OWP.TicketCenter.Handlers
{
    public class TicketTypeRequest : IRequest<IList<TicketTypeModel>>
    {
    }

    public class TicketTypeHandler : IRequestHandler<TicketTypeRequest, IList<TicketTypeModel>>
    {
        private readonly Repository repository;

        public TicketTypeHandler(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<IList<TicketTypeModel>> Handle(TicketTypeRequest request, CancellationToken cancellationToken)
            => await repository.GetTicketTypes();
    }
}