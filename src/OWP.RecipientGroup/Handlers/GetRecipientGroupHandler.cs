namespace OWP.RecipientGroup.Handlers
{
    using MediatR;
    using Models;
    using Services;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;


    public class QueryRecipientGroupRequest : IRequest<IEnumerable<TicketGroup>> { }

    public class GetRecipientGroupHandler : IRequestHandler<QueryRecipientGroupRequest, IEnumerable<TicketGroup>>
    {
        private readonly CommandRepository commandRepository;

        public GetRecipientGroupHandler(CommandRepository commandRepository)
        {
            this.commandRepository = commandRepository;
        }

        public async Task<IEnumerable<TicketGroup>> Handle(QueryRecipientGroupRequest request, CancellationToken cancellationToken)
        {
            var result = await commandRepository.GetRecipientGroup();

            return result.GroupBy(x => new { x.GroupId, x.GroupName }, (key, values) =>
                new TicketGroup
                {
                    GroupId = key.GroupId,
                    GroupName = key.GroupName,
                    Recipients = values.Select(x => new RecipientGroup
                    {
                        RecipientGroupId = x.RecipientGroupId,
                        UserId = x.UserId,
                        Username = x.Username
                    }).ToList()
                }
            );
        }
    }
}