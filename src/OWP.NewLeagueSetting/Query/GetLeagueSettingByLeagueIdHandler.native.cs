using MediatR;

namespace OWP.NewLeagueSetting.Query;

public record GetLeagueSettingByLeagueIdRequest(int owpLeagueId) : IRequest<IEnumerable<LeagueSettingInfoSender>>;

public class GetLeagueSettingByLeagueIdHandler: IRequestHandler<GetLeagueSettingByLeagueIdRequest, IEnumerable<LeagueSettingInfoSender>>
{
    private readonly Repository repository;

    public GetLeagueSettingByLeagueIdHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<LeagueSettingInfoSender>> Handle(GetLeagueSettingByLeagueIdRequest request, CancellationToken cancellationToken)
        => await repository.QueryLeagueSettingByOwpLeagueId(request.owpLeagueId);
}
