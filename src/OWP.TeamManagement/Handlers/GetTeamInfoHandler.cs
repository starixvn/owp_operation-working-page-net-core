using MediatR;
using OWP.TeamManagement.Model;

namespace OWP.TeamManagement.Handlers;

public class GetTeamInfoRequest : IRequest<IEnumerable<QueryTeamInfoResult>>
{
    public string? TeamName { get; set; }
    public string? LeagueName { get; set; } 
    public int LeagueGroupId { get; set; }
}

public class GetTeamInfoHandler(TeamManagementRepository repository) : IRequestHandler<GetTeamInfoRequest, IEnumerable<QueryTeamInfoResult>>
{
    public async Task<IEnumerable<QueryTeamInfoResult>> Handle(GetTeamInfoRequest request, CancellationToken cancellationToken) 
        => await repository.GetTeamInfo(request);
}