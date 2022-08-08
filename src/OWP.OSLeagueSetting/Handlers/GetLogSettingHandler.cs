using MediatR;
using OWP.OSLeagueSetting.Models;
using OWP.OSLeagueSetting.Services;

namespace OWP.OSLeagueSetting.Handlers;


public class GetLogSettingRequest : IRequest<IList<LogDisplayModel>>
{
    public int LeagueSettingId { get; set; }
}

public class Handler : IRequestHandler<GetLogSettingRequest, IList<LogDisplayModel>>
{
    private readonly GetLeagueSettingService getLeagueSettingService;

    public Handler(GetLeagueSettingService getLeagueSettingService) =>
        this.getLeagueSettingService = getLeagueSettingService;

    public async Task<IList<LogDisplayModel>> Handle(GetLogSettingRequest request, CancellationToken cancellationToken)
        => await getLeagueSettingService.BuildLog(request.LeagueSettingId);
}