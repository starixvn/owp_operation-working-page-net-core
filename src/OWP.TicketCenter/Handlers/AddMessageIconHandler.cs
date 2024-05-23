using MediatR;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Handlers;

public class AddMessageIconRequest : MessageIconModel, IRequest<bool>
{

}

public class AddMessageIconHandler : IRequestHandler<AddMessageIconRequest, bool>
{
    private readonly Repository repository;

    public AddMessageIconHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<bool> Handle(AddMessageIconRequest request, CancellationToken cancellationToken)
    {
         await repository.AddMessageIcon(new AddMessageIconCriteria {CreatorId = request.CreatorId, IconKey = request.IconKey, MessageId = request.MessageId} ) ;

        return await Task.FromResult(true);
    }
}