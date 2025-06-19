using MediatR;
using OWP.TeamManagement.Model;

namespace OWP.TeamManagement.Handlers;

public class GetTeamInfoRequest : IRequest<IEnumerable<LeagueInfo>>
{
    public string? TeamName { get; set; }
    public string? LeagueName { get; set; } 
    public int LeagueGroupId { get; set; }
}

public class GetTeamInfoHandler(TeamManagementRepository repository) : IRequestHandler<GetTeamInfoRequest, IEnumerable<LeagueInfo>>
{
    public async Task<IEnumerable<LeagueInfo>> Handle(GetTeamInfoRequest request, CancellationToken cancellationToken) 
        => await repository.GetTeamInfo(request);
}