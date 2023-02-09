using MediatR;
using OWP.LeagueManagement.Models;
using OWP.LeagueManagement.Service;
using OWP.Models;

namespace OWP.LeagueManagement.Handlers;

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
    public async Task<ResultDataObject> Handle(UpdateTvListRequest request, CancellationToken cancellationToken)
        => await leagueManagementService.UpdateTvList(request.Models);
}