using AutoMapper;
using Fanex.NAP.Client;
using OWP.NewLeagueSetting.Service;

namespace OWP.NewLeagueSetting.Command;

public class InsertLeagueSettingRequest : LeagueSettingRequest, Command<int> { }

public class InsertLeagueSettingHandler(Repository repository, INapSession session, IMapper mapper, LogService logService) : CommandHandler<InsertLeagueSettingRequest, int>
{
    public async Task<int> Handle(InsertLeagueSettingRequest request, CancellationToken cancellationToken)
    {
        StandardizedLeagueInfo.BuildMaxbet(request, request.MaxbetWD_HDP, request.MaxbetWK_HDP);
        StandardizedLeagueInfo.BuildNoHdp(request, request.WD_NoOfHDP, request.WK_NoOfHDP);
        StandardizedLeagueInfo.BuildSpread(request, request.Spread);

        var criteria = mapper.Map<InsertLeagueSettingCriteria>(request);
        criteria.ActorId = session.UserId;
        await repository.ExecuteNonQueryCommand(criteria);
        
        var setting = await repository.QueryLeagueSettingDetail(criteria.LeagueSettingId);
        var currentSetting = LoggerServiceImp.MapToChangeModel(new InsertLeagueSettingRequest { });
        var newSetting = LoggerServiceImp.MapToChangeModel(setting);

        await logService.LogSetting(new LogRequest("Created the setting", criteria.LeagueSettingId, currentSetting, newSetting));

        return criteria.LeagueSettingId;
    }
    
}
