using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWP.NewLeagueSetting.Service;
public  interface LogService
{

}

public class LoggerServiceImp(Repository repository) : LogService
{
    public async Task WriteLogUpdateOrDeleteSetting(int leagueSettingId)
    {
        var leagueSetting = await repository.QueryLeagueSettingDetail(leagueSettingId);
        var beforeProcessing = MapToChangeModel(leagueSetting);

        var afterProcessing = new LeagueSettingChangeModel();

        var logData = LeagueSettingChangeService.GetChanges(beforeProcessing, afterProcessing);
        var logDataJson = System.Text.Json.JsonSerializer.Serialize(logData);
    }

    public async Task WriteLogCreateSetting()
    {

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