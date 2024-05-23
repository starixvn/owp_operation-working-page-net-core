

using MediatR;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Handlers;

public class RemoveMessageIconRequest : MessageIconModel, IRequest<bool>
{

}

public class RemoveMessageIconHandler : IRequestHandler<RemoveMessageIconRequest, bool>
{
    private readonly Repository repository;

    public RemoveMessageIconHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<bool> Handle(RemoveMessageIconRequest request, CancellationToken cancellationToken)
    {
        await repository.RemoveMessageIcon(new RemoveMessageIconCriteria { CreatorId = request.CreatorId, IconKey = request.IconKey, MessageId = request.MessageId });

        return await Task.FromResult(true);
    }
}