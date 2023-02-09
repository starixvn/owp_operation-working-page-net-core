namespace OWP.LeagueManagement.Handlers;

using Constants;
using MediatR;
using OWP.Models;

public class GetLeagueGroupRequest : IRequest<IEnumerable<ReferenceResource>>
{

}
public class GetLeagueGroupHandler : IRequestHandler<GetLeagueGroupRequest, IEnumerable<ReferenceResource>>
{
    private readonly Repository repository;
    public GetLeagueGroupHandler(Repository repository)
    {
        this.repository = repository;
    }
    public async Task<IEnumerable<ReferenceResource>> Handle(GetLeagueGroupRequest request, CancellationToken cancellationToken)
    {
        var resource = await repository.GetResourceByName(ReferenceResourceNames.LeagueCategory);
        return resource.OrderBy(x => x.ResourceId);
    }
}