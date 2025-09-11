using OWP.EventPicking.Api.Models;

namespace OWP.EventPicking.Api.Handlers;

public class InsertOrUpdateNoOfferMatchRequest : IRequest<Unit>
{
    public IList<NoOfferMatchInfo> NoOfferMatch { get; set; }

    public bool IsMultiSelected { get; set; } = false;
}

public class InsertOrDeleteNoOfferMatchHandler : IRequestHandler<InsertOrUpdateNoOfferMatchRequest, Unit>
{
    private readonly Repository repository;
    private readonly AccessCounter accessCounter;
    public InsertOrDeleteNoOfferMatchHandler(Repository repository, AccessCounter accessCounter)
    {
        this.repository = repository;
        this.accessCounter = accessCounter;
    }

    public async Task<Unit> Handle(InsertOrUpdateNoOfferMatchRequest request, CancellationToken cancellationToken)
    {
        if (request.IsMultiSelected)
        {
            await accessCounter.UpdateCounter("EventPicking Multiple NoOffer");
        }
        await repository.InsertOrUpdateNoOfferMatch(request);
        return Unit.Value;
    }
}