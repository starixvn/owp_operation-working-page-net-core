using OWP.OSLeagueSetting.Constants;
using OWP.OSLeagueSetting.Models;
using System.Text.RegularExpressions;

namespace OWP.OSLeagueSetting.Helper;

public static class OSLeagueSettingHelper
{
    public static IEnumerable<SummaryDetail> CalculateSummaryInfo(IList<LeagueSettingInfo> leagueSettingInfos)
    {
        var offerLines = leagueSettingInfos.Select(x => x.OfferLines).Distinct();
        var betTypes = leagueSettingInfos
            .Select(x => x.BetType)
            .Distinct()
            .Count(x => !string.IsNullOrEmpty(x));
        var betDeyLay = leagueSettingInfos.Select(x => x.BetDelay).Distinct();
        var summaryInfo = new List<SummaryDetail>
        {
            FormatStyle(offerLines, LeagueSettingConst.OfferLines),
            FormatNumberBetType(betTypes, LeagueSettingConst.BetType),
            FormatStyle(betDeyLay, LeagueSettingConst.BetDelay),
            FormatManualUpdate(leagueSettingInfos, LeagueSettingConst.ManualUpdate)
        };

        return summaryInfo.Where(x => !string.IsNullOrEmpty(x?.Text));
    }

    public static IEnumerable<SummaryDetail> CalculateSummaryRemark(IList<LeagueSettingInfo> leagueSettingInfos)
    {
        var weekMatches = leagueSettingInfos.Select(x => x.WeekMatch).Distinct();
        var providers = leagueSettingInfos.Select(x => x.Provider).Distinct();
        var summaryRemark = new List<SummaryDetail>
        {
            FormatStyle(weekMatches, LeagueSettingConst.WeekMatch),
            FormatStyle(providers, LeagueSettingConst.Provider),
            FormatDateTime(leagueSettingInfos.FirstOrDefault(), LeagueSettingConst.TimeRange),
            FormatManualIpTv(leagueSettingInfos, LeagueSettingConst.IpTv),
            FormatRemark(leagueSettingInfos.FirstOrDefault(), LeagueSettingConst.Remark)
        };

        return summaryRemark.Where(x => !string.IsNullOrEmpty(x?.Text));
    }

    public static string CalculateRoundName(IList<LeagueSettingInfo> leagueSettingInfos)
    {
        var roundNames = leagueSettingInfos.Select(x => x.RoundName)
            .Distinct();
        return string.Join(',', roundNames.Where(x => !string.IsNullOrEmpty(x)));
    }

    public static SummaryDetail FormatStyle(IEnumerable<string> values, string property)
        => new ()
        {
            Text = FormatToString(values, property),
            Style = LeagueSettingConst.SummaryStyleOption.GetValueOrDefault(property)
        };
 

    public static string FormatToString(IEnumerable<string> values, string property)
    {
        var valueList = values.ToList();
        var totalOption = LeagueSettingConst.SummaryFullOption.GetValueOrDefault(property);
        return valueList.Count == totalOption
            ? string.Empty
            : $"{string.Join('/', valueList.Where(x => !string.IsNullOrEmpty(x)))}";
    }

    private static SummaryDetail FormatNumberBetType(int betTypes, string property)
    {
        return new SummaryDetail
        {
            Text = betTypes != 0 ? $"{betTypes} Bet Types" : string.Empty,
            Style = LeagueSettingConst.SummaryStyleOption.GetValueOrDefault(property)
        };
    }

    private static SummaryDetail FormatDateTime(LeagueSettingInfo leagueInfo, string property)
    {
        var style = LeagueSettingConst.SummaryStyleOption.GetValueOrDefault(property);
        if (leagueInfo.KOToTime is null && leagueInfo.KOFromTime is null)
        {
            return null;
        }

        if (leagueInfo.KOFromTime is null)
        {
            return new SummaryDetail
            {
                Text = $"Before {leagueInfo.KOToTime?.ToString(@"hh\:mm")} ",
                Style = style
            };
        }

        if (leagueInfo.KOToTime is null)
        {
            return new SummaryDetail
            {
                Text = $"After {leagueInfo.KOFromTime?.Add(TimeSpan.FromMinutes(-1)):hh\\:mm}",
                Style = style
            };
        }

        return new SummaryDetail
        {
            Text = $"{leagueInfo.KOFromTime?.ToString(@"hh\:mm")} - {leagueInfo.KOToTime?.ToString(@"hh\:mm")}",
            Style = style
        };
    }

    private static SummaryDetail FormatRemark(LeagueSettingInfo leagueInfo, string property)
        => new()
        {
            Text = leagueInfo.Remark,
            Style = LeagueSettingConst.SummaryStyleOption.GetValueOrDefault(property)
        };

    private static SummaryDetail FormatManualUpdate(IEnumerable<LeagueSettingInfo> leagueInfos, string property)
        => new()
        {
            Text = leagueInfos.Any(x => x.IsManualUpdateScore)
                ? $"{LeagueSettingConst.ManualUpdate}"
                : string.Empty,

            Style = LeagueSettingConst.SummaryStyleOption.GetValueOrDefault(property)
        };

    private static SummaryDetail FormatManualIpTv(IEnumerable<LeagueSettingInfo> leagueInfos, string property)
        => new()
        {
            Text = leagueInfos.Any(x => x.IsIptvOnly)
                ? $"{LeagueSettingConst.IpTv}"
                : string.Empty,

            Style = LeagueSettingConst.SummaryStyleOption.GetValueOrDefault(property)
        };

    public static string CalculateSummaryExportFirst(LeagueSettingInfo leagueSettingInfo)
        => (leagueSettingInfo.MaxBetHDP.HasValue ? $"{{HDP {leagueSettingInfo.MaxBetHDP}}}" : string.Empty) +
           (leagueSettingInfo.MaxBetOU.HasValue ? $"{{OU {leagueSettingInfo.MaxBetOU}}}" : string.Empty);

    public static string CalculateSummaryExportSecond(string offerLines, string bettypes, string betDelays)
    {
         string result = "";

        if (!string.IsNullOrEmpty(offerLines))
        {
            result += $"({offerLines})";
        }

        if (!string.IsNullOrEmpty(bettypes))
        {
            result += $"({bettypes})";
        }

        if (!string.IsNullOrEmpty(betDelays))
        {
            result += $"({betDelays})";
        }

        return result;
    }


    public static string CalculateRemarkExport(IList<LeagueSettingInfo> leagueInfos)
    {
        var updateScore = FormatManualUpdate(leagueInfos, LeagueSettingConst.ManualUpdate).Text;
        var weekMatches = leagueInfos.Select(x => x.WeekMatch).Distinct();
        var weekMatchText = FormatToStringExport(weekMatches);
        var providers = leagueInfos.Select(x => x.Provider).Distinct();
        var providerText = FormatToStringExport(providers);
        var koTime = FormatDateTime(leagueInfos.FirstOrDefault(), LeagueSettingConst.TimeRange)?.Text;
        var ipTv = FormatManualIpTv(leagueInfos, LeagueSettingConst.IpTv).Text;
        var remark = FormatRemark(leagueInfos.FirstOrDefault(), LeagueSettingConst.Remark).Text;
        return string.Join("",
            !string.IsNullOrEmpty(updateScore) ? $"({updateScore})" : "",
            !string.IsNullOrEmpty(weekMatchText) ? $"(End Of Preseason before {weekMatchText} Week Match)" : "",
            !string.IsNullOrEmpty(providerText) ? $"({providerText})" : "",
            !string.IsNullOrEmpty(koTime) ? $"({koTime})" : "",
            !string.IsNullOrEmpty(ipTv) ? $"({ipTv})" : "",
            !string.IsNullOrEmpty(remark) ? $"({remark})" : "");
    }

    public static string FormatToStringExport(IEnumerable<string> values)
        => $"{string.Join('/', values.ToList().Where(x => !string.IsNullOrEmpty(x)))}";
}