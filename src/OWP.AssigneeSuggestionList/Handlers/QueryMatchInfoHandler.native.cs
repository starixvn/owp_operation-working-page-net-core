using MediatR;

namespace OWP.AssigneeSuggestionList.Handlers;

using Models;

public class QueryMatchInfoRequest : IRequest<MatchInfo>
{
    public int OwpMatchId { get; set; }

    public string BetType { get; set; }
}

public class QueryMatchInfoHandler : IRequestHandler<QueryMatchInfoRequest, MatchInfo>
{
    private readonly Repository repository;

    public QueryMatchInfoHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<MatchInfo> Handle(QueryMatchInfoRequest request, CancellationToken cancellationToken)
    {
        return await repository.QueryMatchInfo(request.OwpMatchId);
    }
}