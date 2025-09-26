using Fanex.NAP.Client;
using OWP.NewLeagueSetting.Models;
using OWP.NewLeagueSetting.Service;
using System.Linq;

namespace OWP.NewLeagueSetting.Command;

public record RemoveLeagueSettingRequest(int LeagueSettingId) : Command<Unit>;

public class RemoveLeagueSettingHandler(Repository repository, INapSession session, ILeagueSettingChangeService changeService) : CommandHandler<RemoveLeagueSettingRequest, Unit>
{
    
    public async Task<Unit> Handle(RemoveLeagueSettingRequest request, CancellationToken cancellationToken)
    {
        var leagueSetting = await repository.QueryLeagueSettingDetail(request.LeagueSettingId);
        var beforeProcessing = MapToChangeModel(leagueSetting);
        
        var afterProcessing = new LeagueSettingChangeModel();
        
        var LogData = changeService.LogProcessingChanges(beforeProcessing, afterProcessing);
        var LogDataJson = System.Text.Json.JsonSerializer.Serialize(LogData);
        
        await repository.ExecuteNonQueryCommand(new RemoveSettingCriteria()
        {
            LeagueSettingId = request.LeagueSettingId,
            ActorId = session.UserId,
            LogData = LogDataJson
        });
        
        return Unit.Value;
    }
    
    private static LeagueSettingChangeModel MapToChangeModel(LeagueSettingInfoSender leagueSetting)
    {
        return new LeagueSettingChangeModel
        {
            League = leagueSetting.League,
            LeagueGroup = leagueSetting.LeagueGroup,
            Spread = leagueSetting.SpreadShow,
            MaxbetWD_HDP = leagueSetting.MaxbetWD_HDP_Show,
            WD_NoOfHDP = leagueSetting.WD_NoOfHDP_Show,
            MaxbetWK_HDP = leagueSetting.MaxbetWK_HDP_Show,
            WK_NoOfHDP = leagueSetting.WK_NoOfHDP_Show,
            LstNote = leagueSetting.LstNote,
            Remark2 = leagueSetting.Remark2,
            HasTv = leagueSetting.Condition.HasTv,
            HasIpTv = leagueSetting.Condition.HasIpTv,
            IpTv = leagueSetting.Condition.IpTv?.ToList() ?? new List<Condition>(),
            Round = leagueSetting.Condition.Round?.ToList() ?? new List<Condition>(),
            Division = leagueSetting.Condition.Division?.ToList() ?? new List<Condition>(),
            Remark = leagueSetting.Condition.Remark,
            IsBlackList = leagueSetting.IsBlackList,
            NoOfferLive = leagueSetting.NoOfferLive
        };
    }
}
