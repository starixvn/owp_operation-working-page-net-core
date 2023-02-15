namespace OWP.LeagueManagement.Handlers;

using MediatR;
using Models;
using Service;

public class GetConditionRequest : IRequest<LeagueSettingCondition>
{
    public int LeagueSettingId { get; set; }
}
public class GetConditionHandler : IRequestHandler<GetConditionRequest, LeagueSettingCondition>
{
    private readonly LeagueManagementService leagueManagementService;
    public GetConditionHandler(LeagueManagementService leagueManagementService)
    {
        this.leagueManagementService = leagueManagementService;
    }
    public async Task<LeagueSettingCondition> Handle(GetConditionRequest request, CancellationToken cancellationToken)
        => await leagueManagementService.GetCondition(request.LeagueSettingId);

}