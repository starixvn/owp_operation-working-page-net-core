using Fanex.NAP.Client;
using OWP.Extensions;
using OWP.NewLeagueSetting.Command;

namespace OWP.NewLeagueSetting.Service;

public record LogRequest(
    string Action,
    int leagueSettingId,
    LeagueSettingChangeModel CurrentSetting,
    LeagueSettingChangeModel NewSetting);
public  interface LogService
{
    Task LogSetting(LogRequest request);
}

public class LoggerServiceImp(ILeagueSettingChangeService changeSettingService, Repository repository, INapSession session) : LogService
{
    public async Task LogSetting(LogRequest request)
    {
        
        var logData = changeSettingService.LogProcessingChanges(request.CurrentSetting, request.NewSetting);
        var logDataJson = System.Text.Json.JsonSerializer.Serialize(logData);
        await repository.ExecuteNonQueryCommand(new InsertLogCriteria
        {
            Action = request.Action,
            Content = logDataJson,
            CreatedDate = DateTimeOffset.Now.DateTime.ToGmt8(),
            ModifiedBy = session.UserName,
            SettingId = request.leagueSettingId
        });
    }

    
    public static LeagueSettingChangeModel MapToChangeModel(LeagueSettingInfoSender leagueSetting)
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

    public static LeagueSettingChangeModel MapToChangeModel(UpdateLeagueSettingRequest request)
    {
        return new LeagueSettingChangeModel
        {
            League = request.League,
            LeagueGroup = request.LeagueGroup,
            Spread = request.Spread,
            MaxbetWD_HDP = request.MaxbetWD_HDP,
            WD_NoOfHDP = request.WD_NoOfHDP,
            MaxbetWK_HDP = request.MaxbetWK_HDP,
            WK_NoOfHDP = request.WK_NoOfHDP,
            LstNote = request.LstNote,
            Remark2 = request.Remark2,
            HasTv = request.HasTv,
            HasIpTv = request.HasIpTv,
            IpTv = request.IpTv?.ToList() ?? new List<Condition>(),
            Round = request.Round?.ToList() ?? new List<Condition>(),
            Division = request.Division?.ToList() ?? new List<Condition>(),
            Remark = request.Remark,
            IsBlackList = request.IsBlackList,
            NoOfferLive = request.NoOfferLive
        };
    }

    public static LeagueSettingChangeModel MapToChangeModel(InsertLeagueSettingRequest request)
    {
        return new LeagueSettingChangeModel
        {
            League = request.League,
            LeagueGroup = request.LeagueGroup,
            Spread = request.Spread,
            MaxbetWD_HDP = request.MaxbetWD_HDP,
            WD_NoOfHDP = request.WD_NoOfHDP,
            MaxbetWK_HDP = request.MaxbetWK_HDP,
            WK_NoOfHDP = request.WK_NoOfHDP,
            LstNote = request.LstNote,
            Remark2 = request.Remark2,
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