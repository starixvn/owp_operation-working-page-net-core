using MediatR;
using OWP.LeagueManagement.Models;
using OWP.LeagueManagement.Constants;
using OWP.Models;

namespace OWP.LeagueManagement.Handlers
{
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
}