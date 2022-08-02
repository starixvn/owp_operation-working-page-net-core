using MediatR;
using OWP.LeagueManagement.Models;
using OWP.LeagueManagement.Service;
using OWP.Models;

namespace OWP.LeagueManagement.Handlers
{
    public class UpdateConditionRequest : IRequest<ResultDataObject>
    {
        public LeagueSettingCondition Models { get; set; }
    }
    public class UpdateConditionHandler : IRequestHandler<UpdateConditionRequest, ResultDataObject>
    {
        private readonly LeagueManagementService leagueManagementService;
        public UpdateConditionHandler(LeagueManagementService leagueManagementService)
        {
            this.leagueManagementService = leagueManagementService;
        }
        public async Task<ResultDataObject> Handle(UpdateConditionRequest request, CancellationToken cancellationToken)
            => await leagueManagementService.UpdateCondition(request.Models);
    }
}