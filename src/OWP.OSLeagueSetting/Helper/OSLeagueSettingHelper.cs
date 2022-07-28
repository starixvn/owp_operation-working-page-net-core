using OWP.OSLeagueSetting.Constants;
using OWP.OSLeagueSetting.Models;

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
            FormatToString(offerLines, LeagueSettingConst.OfferLines),
            FormatNumberBetType(betTypes, LeagueSettingConst.BetType),
            FormatToString(betDeyLay, LeagueSettingConst.BetDelay),
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
            FormatToString(weekMatches, LeagueSettingConst.WeekMatch),
            FormatToString(providers, LeagueSettingConst.Provider),
            FormatDateTime(leagueSettingInfos.FirstOrDefault(), LeagueSettingConst.TimeRange),
            FormatRemark(leagueSettingInfos.FirstOrDefault(), LeagueSettingConst.Remark),
            FormatManualIpTv(leagueSettingInfos, LeagueSettingConst.IpTv)
        };

        return summaryRemark.Where(x => !string.IsNullOrEmpty(x?.Text));
    }

    private static SummaryDetail FormatToString(IEnumerable<string> values, string property)
    {
        var totalOption = LeagueSettingConst.SummaryFullOption.GetValueOrDefault(property);
        var text = values.Count() == totalOption
            ? string.Empty
            : $"{string.Join('/', values.Where(x => !string.IsNullOrEmpty(x)))}";

        return new()
        {
            Text = text,
            Style = LeagueSettingConst.SummaryStyleOption.GetValueOrDefault(property)
        };
    }

    private static SummaryDetail FormatNumberBetType(int betTypes, string property)
    {
        return new()
        {
            Text = $"{betTypes} Bet Types",
            Style = LeagueSettingConst.SummaryStyleOption.GetValueOrDefault(property)
        };
    }

    private static SummaryDetail FormatDateTime(LeagueSettingInfo leagueInfo, string property)
    {
        if (leagueInfo.KOToTime is null && leagueInfo.KOFromTime is null)
        {
            return null;
        }

        return new()
        {
            Text = $"{leagueInfo.KOFromTime?.ToString(@"hh\:mm")} - {leagueInfo.KOToTime?.ToString(@"hh\:mm")}",
            Style = LeagueSettingConst.SummaryStyleOption.GetValueOrDefault(property)
        };
    }

    private static SummaryDetail FormatRemark(LeagueSettingInfo leagueInfo, string property)
        => new()
        {
            Text = leagueInfo.Remark,
            Style = LeagueSettingConst.SummaryStyleOption.GetValueOrDefault(property)
        };

    private static SummaryDetail FormatManualUpdate(IList<LeagueSettingInfo> leagueInfos, string property)
        => new()
        {
            Text = leagueInfos.Any(x => x.IsManualUpdateScore)
                ? $"{LeagueSettingConst.ManualUpdate}"
                : string.Empty,

            Style = LeagueSettingConst.SummaryStyleOption.GetValueOrDefault(property)
        };

    private static SummaryDetail FormatManualIpTv(IList<LeagueSettingInfo> leagueInfos, string property)
        => new()
        {
            Text = leagueInfos.Any(x => x.IsIptvOnly)
                ? $"{LeagueSettingConst.IpTv}"
                : string.Empty,

            Style = LeagueSettingConst.SummaryStyleOption.GetValueOrDefault(property)
        };
}