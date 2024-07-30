using MediatR;
using OWP.TicketSetting.TicketEvent.Models;

namespace OWP.TicketSetting.TicketEvent.Handlers;

public class GetTicketListRequest : IRequest<IList<TicketListModel>>
{
    public int OwpMatchId { get; set; }

    public int ActorId { get; set; }
}

public class GetTicketListHandler : IRequestHandler<GetTicketListRequest, IList<TicketListModel>>
{
    private readonly Repository repository;

    public GetTicketListHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<IList<TicketListModel>> Handle(GetTicketListRequest request, CancellationToken cancellationToken)
    {
        return await repository.GetTicketFromDb(request);
    }

}
