using MediatR;
using OWP.EventPicking.Models;
using OWP.Services;

namespace OWP.EventPicking.Handlers;

public class InsertOrDeleteNoOfferMatchRequest : IRequest<Unit>
{
    public IList<NoOfferMatchInfo> NoOfferMatch { get; set; }

    public bool IsMultiSelected { get; set; } = false;
}

public class InsertOrDeleteNoOfferMatchHandler : IRequestHandler<InsertOrDeleteNoOfferMatchRequest, Unit>
{
    private readonly Repository repository;
    private readonly AccessCounter accessCounter;
    public InsertOrDeleteNoOfferMatchHandler(Repository repository , AccessCounter accessCounter)
    {
        this.repository = repository;
        this.accessCounter = accessCounter;
    }

    public async Task<Unit> Handle(InsertOrDeleteNoOfferMatchRequest request, CancellationToken cancellationToken)
    {
        if (request.IsMultiSelected)
        {
          await  accessCounter.UpdateCounter("EventPicking Multiple NoOffer");
        }
        await repository.InsertOrDeleteNoOfferMatch(request);
        return Unit.Value;
    }
}