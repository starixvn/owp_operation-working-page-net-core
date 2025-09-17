using OWP.EventPicking.Api.Models;

namespace OWP.EventPicking.Api.Handlers;

public class InsertOrUpdateNoOfferMatchRequest : IRequest<Unit>
{
    public IList<NoOfferMatchInfo> NoOfferMatch { get; set; }

    public bool IsMultiSelected { get; set; } = false;
}

public class InsertOrDeleteNoOfferMatchHandler(Repository repository, AccessCounter accessCounter)
    : IRequestHandler<InsertOrUpdateNoOfferMatchRequest, Unit>
{
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