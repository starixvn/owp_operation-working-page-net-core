using AutoMapper;
using Fanex.NAP.Client;
using OWP.NewLeagueSetting.Service;

namespace OWP.NewLeagueSetting.Command;

public class InsertLeagueSettingRequest : LeagueSettingRequest, Command<int> { }

public class InsertLeagueSettingHandler(Repository repository, INapSession session, IMapper mapper, ILeagueSettingChangeService changeService) : CommandHandler<InsertLeagueSettingRequest, int>
{
    public async Task<int> Handle(InsertLeagueSettingRequest request, CancellationToken cancellationToken)
    {
        
        var beforeProcessing = new InsertLeagueSettingRequest { };
        
        StandardizedLeagueInfo.BuildMaxbet(request, request.MaxbetWD_HDP, request.MaxbetWK_HDP);
        StandardizedLeagueInfo.BuildNoHdp(request, request.WD_NoOfHDP, request.WK_NoOfHDP);
        StandardizedLeagueInfo.BuildSpread(request, request.Spread);
        
        
        var LogData = LogProcessingChanges(beforeProcessing, request);
        var LogDataJson = System.Text.Json.JsonSerializer.Serialize(LogData);
        
        var criteria = mapper.Map<InsertLeagueSettingCriteria>(request);
        criteria.ActorId = session.UserId;
        criteria.LogData = LogDataJson;
        await repository.ExecuteNonQueryCommand(criteria);
        return criteria.LeagueSettingId;
    }
    
    private List<LogDetail> LogProcessingChanges(InsertLeagueSettingRequest before, InsertLeagueSettingRequest after)
    {
        var beforeModel = MapToChangeModel(before);
        var afterModel = MapToChangeModel(after);
        return changeService.LogProcessingChanges(beforeModel, afterModel);
    }
    
    private static LeagueSettingChangeModel MapToChangeModel(InsertLeagueSettingRequest request)
    {
        return new LeagueSettingChangeModel
        {
            LeagueGroupId = request.LeagueGroupId,
            League = request.League,
            LeagueGroup = request.LeagueGroup,
            Spread = request.Spread,
            MaxbetWD_HDP = request.MaxbetWD_HDP,
            WD_NoOfHDP = request.WD_NoOfHDP,
            MaxbetWK_HDP = request.MaxbetWK_HDP,
            WK_NoOfHDP = request.WK_NoOfHDP,
            LstNote = request.LstNote,
            Remark2 = request.Remark2,
            Comment = request.Comment,
            HasTv = request.HasTv,
            HasIpTv = request.HasIpTv,
            IpTv = request.IpTv,
            Round = request.Round,
            Division = request.Division,
            Remark = request.Remark,
            IsBlackList = request.IsBlackList,
            NoOfferLive = request.NoOfferLive
        };
    }
    
}
