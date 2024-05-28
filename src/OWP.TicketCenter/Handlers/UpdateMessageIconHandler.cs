using MediatR;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Handlers;

public class UpdateMessageIconRequest : MessageIconModel, IRequest<bool>
{

}

public class UpdateMessageIconHandler : IRequestHandler<UpdateMessageIconRequest, bool>
{
    private readonly Repository repository;

    public UpdateMessageIconHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<bool> Handle(UpdateMessageIconRequest request, CancellationToken cancellationToken)
    {
        await repository.AddMessageIcon(new UpdateMessageIconCriteria { CreatorId = request.CreatorId, IconKey = request.IconKey, MessageId = request.MessageId });

        return await Task.FromResult(true);
    }
}