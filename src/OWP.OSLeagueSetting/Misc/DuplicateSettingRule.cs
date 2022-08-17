using OWP.Extensions;
using OWP.OSLeagueSetting.Models;

namespace OWP.OSLeagueSetting.Misc;

public class DuplicateRule
{
    private DuplicateRule nextRule;

    public DuplicateRule SetNextRule(DuplicateRule rule)
    {
        nextRule = rule;
        return nextRule;
    }

    public virtual bool IsDuplicate(SettingDetail model, IList<DuplicateSettingModel> leagueSettings)
    {
        return nextRule?.IsDuplicate(model, leagueSettings) ?? true;
    }
}

public class RoundRule : DuplicateRule
{
    public override bool IsDuplicate(SettingDetail model, IList<DuplicateSettingModel> leagueSettings)
    {
        var leaguesByRound = GetLeagueSettingByRound(model.Rounds, leagueSettings);

        return leaguesByRound.Any() && base.IsDuplicate(model, leagueSettings);
    }

    private static IList<DuplicateSettingModel> GetLeagueSettingByRound(string round,
        IList<DuplicateSettingModel> leagueSettings)
    {
        var roundIds = round.Split(',').Select(i => i.ConvertNullableInt());

        return leagueSettings.Where(x => roundIds.Intersect(x.RoundIds).Any())
            .ToList();
    }
}

public class OfferLinesRule : DuplicateRule
{
    public override bool IsDuplicate(SettingDetail model, IList<DuplicateSettingModel> leagueSettings)
    {
        var leaguesByOfferLine = GetLeagueSettingByRound(model.OfferLines, leagueSettings);

        return leaguesByOfferLine.Any() && base.IsDuplicate(model, leagueSettings);
    }

    private static IList<DuplicateSettingModel> GetLeagueSettingByRound(string offerLines,
        IList<DuplicateSettingModel> leagueSettings)
    {
        var offerLineIds = offerLines.Split(',').Select(i => i.ConvertNullableInt());

        return leagueSettings.Where(x => offerLineIds.Intersect(x.OfferLinesIds).Any())
                .ToList();
    }
}

public class BetDelayRule : DuplicateRule
{
    public override bool IsDuplicate(SettingDetail model, IList<DuplicateSettingModel> leagueSettings)
    {
        var leaguesByOfferLine = GetLeagueSettingByBetDelay(model.BetDelay, leagueSettings);

        return leaguesByOfferLine.Any() && base.IsDuplicate(model, leagueSettings);
    }

    private static IList<DuplicateSettingModel> GetLeagueSettingByBetDelay(string betDelay,
        IList<DuplicateSettingModel> leagueSettings)
    {
        var betDelayIds = betDelay.Split(',').Select(i => i.ConvertNullableInt());

        return leagueSettings.Where(x => betDelayIds.Intersect(x.BetDeLayIds).Any())
                .ToList();
    }
}

public class ProviderRule : DuplicateRule
{
    public override bool IsDuplicate(SettingDetail model, IList<DuplicateSettingModel> leagueSettings)
    {
        var leaguesByOfferLine = GetLeagueSettingByProvider(model.Providers, leagueSettings);

        return leaguesByOfferLine.Any() && base.IsDuplicate(model, leagueSettings);
    }

    private static IList<DuplicateSettingModel> GetLeagueSettingByProvider(string provider,
        IList<DuplicateSettingModel> leagueSettings)
    {
        var providerIds = provider.Split(',').Select(i => i.ConvertNullableInt());

        return leagueSettings.Where(x => providerIds.Intersect(x.ProviderIds).Any())
               .ToList();
    }
}

public class RangeTimeRule : DuplicateRule
{
    public override bool IsDuplicate(SettingDetail model, IList<DuplicateSettingModel> leagueSettings)
    {
        var leaguesByOfferLine = GetLeagueSettingByTimeRange(model.KOFromTime?.TimeOfDay, model.KOToTime?.TimeOfDay, leagueSettings);

        return leaguesByOfferLine.Any() && base.IsDuplicate(model, leagueSettings);
    }

    private static IList<DuplicateSettingModel> GetLeagueSettingByTimeRange(TimeSpan? koFromTime, TimeSpan? koToTime, IList<DuplicateSettingModel> leagueSettings)
    {
        return leagueSettings.Where(x => leagueSettings.Any(x => IsTimeRangeOverLap(
                koFromTime.GetValueOrDefault(), koToTime.GetValueOrDefault(),
                x.KOFromTime.GetValueOrDefault(), x.KOToTime.GetValueOrDefault()))).ToList();
    }

    private static bool IsTimeRangeOverLap(TimeSpan fromTime, TimeSpan toTime, TimeSpan fromTimeDB, TimeSpan toTimeDB)
    {
        var newKoToTimeModel = GetFormattedKoToTime(fromTime, toTime);
        var newKoToTimeDB = GetFormattedKoToTime(fromTimeDB, toTimeDB);
        return fromTime <= newKoToTimeDB
               && fromTimeDB <= newKoToTimeModel;
    }

    private static TimeSpan GetFormattedKoToTime(TimeSpan fromTime, TimeSpan toTime)
    {
        return toTime < fromTime ? toTime += TimeSpan.FromHours(24) : toTime;
    }
}

public class IPTVRule : DuplicateRule
{
    public override bool IsDuplicate(SettingDetail model, IList<DuplicateSettingModel> leagueSettings)
    {
        var leaguesByIpTv = GetLeagueSettingByIPTV(model.IsIptvOnly, leagueSettings);

        return leaguesByIpTv.Any() && base.IsDuplicate(model, leagueSettings);
    }

    private static IList<DuplicateSettingModel> GetLeagueSettingByIPTV(bool isIptv,
        IList<DuplicateSettingModel> leagueSettings)
    {
        return leagueSettings.Where(x => x.IsIptvOnly == isIptv)
            .ToList();
    }
}

public class RemarkRule : DuplicateRule
{
    public override bool IsDuplicate(SettingDetail model, IList<DuplicateSettingModel> leagueSettings)
    {
        var leaguesByIpTv = GetLeagueSettingByRemark(model.Remark, leagueSettings);

        return leaguesByIpTv.Any() && base.IsDuplicate(model, leagueSettings);
    }

    private static IList<DuplicateSettingModel> GetLeagueSettingByRemark(string remark,
        IList<DuplicateSettingModel> leagueSettings)
    {
        return leagueSettings.Where(x => x.Remark == remark)
            .ToList();
    }
}