using MediatR;
using OWP.EventPicking.Models;

namespace OWP.EventPicking.Handlers;

public class InsertOrDeleteNoOfferMatchRequest : IRequest<Unit>
{
    public IList<NoOfferMatchInfo> NoOfferMatch { get; set; }
}

public class InsertOrDeleteNoOfferMatchHandler : IRequestHandler<InsertOrDeleteNoOfferMatchRequest, Unit>
{
    private readonly Repository repository;

    public InsertOrDeleteNoOfferMatchHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<Unit> Handle(InsertOrDeleteNoOfferMatchRequest request, CancellationToken cancellationToken)
    {
        await repository.InsertOrDeleteNoOfferMatch(request);
        return Unit.Value;
    }
}