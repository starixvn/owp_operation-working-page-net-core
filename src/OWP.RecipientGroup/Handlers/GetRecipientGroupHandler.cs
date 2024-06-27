using MediatR;
using OWP.Enums;
using OWP.RecipientGroup.Models;
using OWP.RecipientGroup.Services;

namespace OWP.RecipientGroup.Handlers;

public class QueryRecipientGroupRequest : IRequest<IEnumerable<TicketGroup>>;

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
                Recipients = values.Select(x => new Recipient
                    {
                        RecipientGroupId = x.RecipientGroupId,
                        RecipientId = x.UserId,
                        RecipientName = x.Username,
                        RecipientType = (int)TicketRecipient.USER
                    })
                    .OrderBy(x => x.RecipientName)
                    .ToList()
            }
        ).OrderBy(x => x.GroupName);
    }
}