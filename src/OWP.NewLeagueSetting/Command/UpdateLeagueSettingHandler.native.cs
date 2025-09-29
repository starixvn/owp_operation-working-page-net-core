using Fanex.NAP.Client;
using OWP.NewLeagueSetting.Service;

namespace OWP.NewLeagueSetting.Command;

public class UpdateLeagueSettingRequest : LeagueSettingRequest, Command<Unit> { }

public class UpdateLeagueSettingHandler(Repository repository, INapSession session, LogService logService) : CommandHandler<UpdateLeagueSettingRequest, Unit>
{
    public async Task<Unit> Handle(UpdateLeagueSettingRequest request, CancellationToken cancellationToken)
    {
        StandardizedLeagueInfo.BuildMaxbet(request, request.MaxbetWD_HDP, request.MaxbetWK_HDP);
        StandardizedLeagueInfo.BuildNoHdp(request, request.WD_NoOfHDP, request.WK_NoOfHDP);
        StandardizedLeagueInfo.BuildSpread(request, request.Spread);

        var leagueSetting = await repository.QueryLeagueSettingDetail(request.LeagueSettingId);
        var currentSetting = LoggerServiceImp.MapToChangeModel(leagueSetting);
        var newSetting = LoggerServiceImp.MapToChangeModel(request);
        
        await repository.UpdateLeagueSetting(request, session.UserId);

        await logService.LogSetting(new LogRequest("Updated the setting", request.LeagueSettingId, currentSetting, newSetting));

        return Unit.Value;
    }
}
