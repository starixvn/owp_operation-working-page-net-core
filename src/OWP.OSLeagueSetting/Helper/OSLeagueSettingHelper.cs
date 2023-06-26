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

    private static SummaryDetail FormatToString(IEnumerable<string> values, string property)
    {
        var valueList = values.ToList();
        var totalOption = LeagueSettingConst.SummaryFullOption.GetValueOrDefault(property);
        var text = valueList.Count == totalOption
            ? string.Empty
            : $"{string.Join('/', valueList.Where(x => !string.IsNullOrEmpty(x)))}";

        return new SummaryDetail
        {
            Text = text,
            Style = LeagueSettingConst.SummaryStyleOption.GetValueOrDefault(property)
        };
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
}