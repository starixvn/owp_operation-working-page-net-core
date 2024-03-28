using MediatR;
using OWP.OtherSport.EventAssignment.Services;

namespace OWP.OtherSport.EventAssignment;
public class QueryLeagueIdRequest : IRequest<int>
{
    public int AdminSiteMatchId { get; set; }
}

public class QueryLeagueId : IRequestHandler<QueryLeagueIdRequest, int>
{
    private readonly CommandRepository commandRepository;

    public QueryLeagueId(CommandRepository commandRepository)
    {
        this.commandRepository = commandRepository;
    }

    public async Task<int> Handle(QueryLeagueIdRequest request, CancellationToken cancellationToken)
        => await commandRepository.QueryLeagueIdByAdminSiteId(request.AdminSiteMatchId);
}
