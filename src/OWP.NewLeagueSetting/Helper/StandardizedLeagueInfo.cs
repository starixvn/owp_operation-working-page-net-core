using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace OWP.NewLeagueSetting.Helper;

public static partial class StandardizedLeagueInfo
{
    public static IList<LeagueSettingInfo> Execute(IList<LeagueSettingExcelInfo> excelInfo)
    {
        var leagueInfo = new List<LeagueSettingInfo>();
        foreach (var info in excelInfo)
        {
            var setting = new LeagueSettingInfo();
            BuildCommonInfo(setting, info);
            BuildCondition(setting, info);
            BuildSpread(setting, info.FTSpread);
            BuildMaxbet(setting, info);
            BuildMNoHdp(setting, info);
            leagueInfo.Add(setting);
        }
        return leagueInfo;
    }

    private static void BuildCommonInfo(LeagueSettingInfo leagueInfo, LeagueSettingExcelInfo excelInfo)
    {
        leagueInfo.LeagueGroup = excelInfo.LeagueGroup;
        leagueInfo.League = excelInfo.League;
        leagueInfo.LstNote = excelInfo.LstNote;
        leagueInfo.Remark2 = excelInfo.Remark2;
    }
    private static void BuildCondition(LeagueSettingInfo leagueInfo, LeagueSettingExcelInfo excelInfo)
    {
        leagueInfo.FullConditionDescription = JsonConvert.SerializeObject(new LeagueSettingCondition
        {
            HasTv = false,
            IpTv = AnalyzeConditionString(excelInfo.LocalTv.Trim()),
            Round = AnalyzeConditionString(excelInfo.Round.Trim()),
            Division = AnalyzeConditionString(excelInfo.LeagueDivision.Trim()),
            Remark = excelInfo.Remark
        });
    }
    private static void BuildSpread(LeagueSettingInfo leagueInfo, string ftSpread)
    {
        (decimal? spreadHdp, decimal? spreadOu, string spreadSource) = AnalyzeSpreadFT(ftSpread);
        leagueInfo.Spread_HDP_value = spreadHdp;
        leagueInfo.Spread_OU_value = spreadOu;
        leagueInfo.Spread_Note = spreadSource;
    }
    private static void BuildMaxbet(LeagueSettingInfo leagueInfo, LeagueSettingExcelInfo excelInfo)
    {
        (decimal? maxbetWd, string maxbetSourceWd) = AnalyzeMaxbet(excelInfo.WD_HDP);
        leagueInfo.MaxbetWD_HDP_value = maxbetWd;
        leagueInfo.MaxbetWD_HDP_Note = maxbetSourceWd;

        (decimal? maxbetWk, string maxbetSourceWk) = AnalyzeMaxbet(excelInfo.WK_HDP);
        leagueInfo.MaxbetWD_HDP_value = maxbetWk;
        leagueInfo.MaxbetWD_HDP_Note = maxbetSourceWk;
    }

    private static void BuildMNoHdp(LeagueSettingInfo leagueInfo, LeagueSettingExcelInfo excelInfo)
    {
        (string noHdpWd, string noHdpWdSource) = AnalyzeNoHdp(excelInfo.WD_NO_HDP);
        leagueInfo.MaxbetWD_NoOfHDP_value = noHdpWd;
        leagueInfo.MaxbetWD_NoOfHDP_Note = noHdpWdSource;

        (string noHdpWk, string noHdpWkSource) = AnalyzeNoHdp(excelInfo.WK_NO_HDP);
        leagueInfo.MaxbetWK_NoOfHDP_value = noHdpWk;
        leagueInfo.MaxbetWK_NoOfHDP_Note = noHdpWkSource;
    }

    private static (decimal? maxbet, string maxbetSource) AnalyzeMaxbet(string maxbetSource)
    {
        var regex = Pattern.MaxbetRegex();
        if (regex.IsMatch(maxbetSource))
        {
            var maxbet = RemoveLastCharacterAndParseToNumber(maxbetSource);

            return (maxbet * 1000, null);
        }
        return (null, maxbetSource);
    }
    private static (string noHdp, string noHdpSource) AnalyzeNoHdp(string noHdpSource)
    {
        var regex = Pattern.NoHDPRegex();
        return regex.IsMatch(noHdpSource)
        ? (noHdpSource, null)
        : (null, noHdpSource);
    }

    private static List<Condition> AnalyzeConditionString(string conditionSource)
    {
        var condition = new List<Condition>();
        foreach (var value in conditionSource?.Split("/"))
        {
            var type = value.StartsWith("*") && value.EndsWith("*") ?
            "Contains"
            : "Equals";

            condition.Add(new(type, value));
        }
        return condition;
    }

    private static (decimal? spreadHdp, decimal? spreadOu, string spreadSource) AnalyzeSpreadFT(string spreadSource)
    {
        var regex = Pattern.SpreadRegex();
        if (regex.IsMatch(spreadSource.Trim()))
        {
            var spreadSplit = spreadSource.Split("-");
            var spreadHdp = RemoveLastCharacterAndParseToNumber(spreadSplit.First());
            var spreadOu = RemoveLastCharacterAndParseToNumber(spreadSplit.Last());

            return (spreadHdp / 1000, spreadOu / 1000, null);
        }

        return (null, null, spreadSource);
    }

    private static int RemoveLastCharacterAndParseToNumber(string source)
    {
        int.TryParse(source?.Remove(source.Length - 1), out int spread);
        return spread;
    }
}