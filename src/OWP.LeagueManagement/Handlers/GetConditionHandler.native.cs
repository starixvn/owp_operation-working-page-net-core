using MediatR;
using OWP.LeagueManagement.Models;
using OWP.LeagueManagement.Service;

namespace OWP.LeagueManagement.Handlers
{
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
}