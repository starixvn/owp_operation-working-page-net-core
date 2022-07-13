using MediatR;
using OWP.LeagueManagement.Models;
using OWP.LeagueManagement.Service;
using OWP.LeagueManagement.Constants;

namespace OWP.LeagueManagement.Handlers
{
    public class GetLeagueGroupRequest : IRequest<IEnumerable<ReferenceResource>>
    {

    }
    public class GetLeagueGroupHandler : IRequestHandler<GetLeagueGroupRequest, IEnumerable<ReferenceResource>>
    {
        private readonly LeagueManagementService leagueManagementService;
        public GetLeagueGroupHandler(LeagueManagementService leagueManagementService)
        {
            this.leagueManagementService = leagueManagementService;
        }
        public async Task<IEnumerable<ReferenceResource>> Handle(GetLeagueGroupRequest request, CancellationToken cancellationToken)
        {
            var resource = await leagueManagementService.GetResourceByName(ReferenceResourceNames.LeagueCategory);
            return resource.OrderBy(x => x.ResourceId);
        }
    }
}