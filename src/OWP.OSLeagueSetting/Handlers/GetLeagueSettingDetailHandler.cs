using MediatR;
using OWP.OSLeagueSetting.Models;

namespace OWP.OSLeagueSetting.Handlers;

public class GetLeagueSettingDetailRequest : IRequest<SettingDetail>
{
    public int LeagueSettingId { get; set; }
}

public class GetLeagueSettingDetailHandler : IRequestHandler<GetLeagueSettingDetailRequest, SettingDetail>
{
    private readonly Repository repository;

    public GetLeagueSettingDetailHandler(Repository repository) => this.repository = repository;

    public async Task<SettingDetail> Handle(GetLeagueSettingDetailRequest request, CancellationToken cancellationToken)
        => await repository.GetLeagueSettingDetail(request.LeagueSettingId);
}