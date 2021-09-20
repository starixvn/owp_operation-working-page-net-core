using OWP.RecipientGroup.Services;

namespace OWP.RecipientGroup.Handlers
{
    using MediatR;
    using Models;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class QueryRecipientGroupRequest : IRequest<IEnumerable<TicketGroup>> { }

    public class GroupHandler : IRequestHandler<QueryRecipientGroupRequest, IEnumerable<TicketGroup>>
    {
        private readonly CommandRepository commandRepository;

        public GroupHandler(CommandRepository commandRepository)
        {
            this.commandRepository = commandRepository;
        }

        public async Task<IEnumerable<TicketGroup>> Handle(QueryRecipientGroupRequest request, CancellationToken cancellationToken)
        {
            return await commandRepository.GetRecipientGroup();
        }
    }
}