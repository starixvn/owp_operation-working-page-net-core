using OWP.LeagueManagement.Services;

namespace OWP.LeagueManagement.Handlers;

using MediatR;
using Models;
using OWP.Models;

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