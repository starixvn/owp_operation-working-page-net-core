using System.Reflection.Metadata;
using MediatR;
using OWP.LeagueManagement.Models;
using OWP.LeagueManagement.Service;
using OWP.LeagueManagement.Constants;

namespace OWP.LeagueManagement.Handlers
{
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

        public Task<IList<LeagueInfoResponse>> Handle(GetAllLeagueRequest request, CancellationToken cancellationToken)
         => leagueManagementService.GetAllLeague(request.Query);
    }
}