using OWP.LeagueManagement.Services;

namespace OWP.LeagueManagement.Handlers;

using MediatR;
using Models;

public class GetAllLeagueRequest : IRequest<IList<LeagueInfoResponse>>
{
    public LeagueInfoQuery Query { get; set; }
}

public class GetAllLeagueHandler : IRequestHandler<GetAllLeagueRequest, IList<LeagueInfoResponse>>
{
    private readonly LeagueManagementService leagueManagementService;

    public GetAllLeagueHandler(LeagueManagementService leagueManagementService)
    {
        this.leagueManagementService = leagueManagementService;
    }

    public async Task<IList<LeagueInfoResponse>> Handle(GetAllLeagueRequest request, CancellationToken cancellationToken)
        => await leagueManagementService.GetAllLeague(request.Query);
}