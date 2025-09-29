using Fanex.NAP.Client;
using OWP.NewLeagueSetting.Service;

namespace OWP.NewLeagueSetting.Command;

public record RemoveLeagueSettingRequest(int LeagueSettingId) : Command<Unit>;

public class RemoveLeagueSettingHandler(Repository repository, INapSession session,
    LogService logService) : CommandHandler<RemoveLeagueSettingRequest, Unit>
{
    
    public async Task<Unit> Handle(RemoveLeagueSettingRequest request, CancellationToken cancellationToken)
    {
        var leagueSetting = await repository.QueryLeagueSettingDetail(request.LeagueSettingId);
        var currentSetting = LoggerServiceImp.MapToChangeModel(leagueSetting);
        
        await repository.ExecuteNonQueryCommand(new RemoveSettingCriteria()
        {
            LeagueSettingId = request.LeagueSettingId,
            ActorId = session.UserId
        });
        
        await logService.LogUpdateOrDeleteSetting(new LogRequest("Created the setting",request.LeagueSettingId, currentSetting, new LeagueSettingChangeModel()));
        return Unit.Value;
    }
}
