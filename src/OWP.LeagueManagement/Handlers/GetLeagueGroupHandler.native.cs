using MediatR;
using OWP.LeagueManagement.Constants;
using OWP.Models;
using OWP.Services;

namespace OWP.LeagueManagement.Handlers;

public class GetLeagueGroupRequest : IRequest<IEnumerable<ReferenceResource>>
{

}
public class GetLeagueGroupHandler : IRequestHandler<GetLeagueGroupRequest, IEnumerable<ReferenceResource>>
{
    private readonly RefResourceService refResourceService;
    public GetLeagueGroupHandler(RefResourceService refResourceService)
    {
        this.refResourceService = refResourceService;
    }
    public async Task<IEnumerable<ReferenceResource>> Handle(GetLeagueGroupRequest request, CancellationToken cancellationToken)
    {
        var resource = await refResourceService.GetRefResourcesByName(ReferenceResourceNames.LeagueCategory);
        return resource.OrderBy(x => x.ResourceId);
    }
}