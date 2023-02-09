namespace OWP.TicketCenter.Handlers;

using MediatR;
using OWP.TicketCenter.Middleware.Models;
using System.Threading;
using System.Threading.Tasks;

public class SaveMessageRequest : PermissionCheckedModel, IRequest<bool>
{
    public int MessageId { get; set; }

    public bool IsSaved { get; set; }
}

public class SaveMessageHandler : IRequestHandler<SaveMessageRequest, bool>
{
    private readonly Repository repository;

    public SaveMessageHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<bool> Handle(SaveMessageRequest request, CancellationToken cancellationToken)
    {
        await repository.UpdateIsSavedMessage(request);

        return await Task.FromResult(true);
    }
}