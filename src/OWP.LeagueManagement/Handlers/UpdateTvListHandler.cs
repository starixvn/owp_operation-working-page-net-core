using MediatR;
using OWP.LeagueManagement.Models;
using OWP.LeagueManagement.Service;
using OWP.LeagueManagement.Constants;
using OWP.Models;

namespace OWP.LeagueManagement.Handlers
{
    public class UpdateTvListRequest : IRequest<ResultDataObject>
    {
        public IEnumerable<TVList> Models { get; set; }
    }
    public class UpdateTvListHandler : IRequestHandler<UpdateTvListRequest, ResultDataObject>
    {
        private readonly LeagueManagementService leagueManagementService;
        public UpdateTvListHandler(LeagueManagementService leagueManagementService)
        {
            this.leagueManagementService = leagueManagementService;
        }
        public Task<ResultDataObject> Handle(UpdateTvListRequest request, CancellationToken cancellationToken)
        => leagueManagementService.UpdateTvList(request.Models);
    }
}