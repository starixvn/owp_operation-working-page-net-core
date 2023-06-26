using MediatR;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Handlers;

public class GetMatchInfoByAdminSiteIdRequest : IRequest<MatchInfo>
{
    public int AdminSiteId { get; set; }
}

public class GetMatchInfoByAdminSiteIdHandler : IRequestHandler<GetMatchInfoByAdminSiteIdRequest, MatchInfo>
{
    private readonly Repository repository;

    public GetMatchInfoByAdminSiteIdHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<MatchInfo> Handle(GetMatchInfoByAdminSiteIdRequest request,
        CancellationToken cancellationToken)
    {
        return await repository.GetMatchInfoByAdminSiteId(request.AdminSiteId);
    }
}