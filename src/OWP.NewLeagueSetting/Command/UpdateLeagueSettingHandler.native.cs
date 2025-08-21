using Fanex.NAP.Client;

namespace OWP.NewLeagueSetting.Command;

public class UpdateLeagueSettingRequest : LeagueSettingRequest, Command<Unit> { }

public class UpdateLeagueSettingHandler(Repository repository, INapSession session) : CommandHandler<UpdateLeagueSettingRequest, Unit>
{
    public async Task<Unit> Handle(UpdateLeagueSettingRequest request, CancellationToken cancellationToken)
    {
        StandardizedLeagueInfo.BuildMaxbet(request, request.MaxbetWD_HDP, request.MaxbetWK_HDP);
        StandardizedLeagueInfo.BuildNoHdp(request, request.WD_NoOfHDP, request.WK_NoOfHDP);
        StandardizedLeagueInfo.BuildSpread(request, request.Spread);
        await repository.UpdateLeagueSetting(request, session.UserId);
        return Unit.Value;
    }
}
