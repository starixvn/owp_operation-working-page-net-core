using Fanex.NAP.Client.Helpers.Web;
using MediatR;
using OWP.TicketEvent.Models;

namespace OWP.TicketEvent.Handlers;

public class GetTicketListRequest : IRequest<IList<TicketListModel>>
{
    public int OwpMatchId { get; set; }

    public int ActorId { get; set; }
}

public class GetTicketListHandler : IRequestHandler<GetTicketListRequest, IList<TicketListModel>>
{
    private readonly Repository repository;

    private readonly INapSession currentSession;

    public GetTicketListHandler(Repository repository, INapSession currentSession)
    {
        this.repository = repository;
        this.currentSession = currentSession;
    }

    public async Task<IList<TicketListModel>> Handle(GetTicketListRequest request, CancellationToken cancellationToken)
    {
        return await repository.GetTicketFromDb(request);
    }

}
