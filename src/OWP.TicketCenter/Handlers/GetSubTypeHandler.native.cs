using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Handlers
{
    public class GetSubTypeRequest : IRequest<IEnumerable<SubTypeModel>>
    {
        public int TicketTypeId { get; set; }
    }
    public class GetSubTypeHandler : IRequestHandler<GetSubTypeRequest, IEnumerable<SubTypeModel>>
    {
        private readonly Repository repository;

        public GetSubTypeHandler(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<SubTypeModel>> Handle(GetSubTypeRequest request, CancellationToken cancellationToken)
          => (await repository.GetSubTypeByTicketTypeId(request.TicketTypeId));
    }
}