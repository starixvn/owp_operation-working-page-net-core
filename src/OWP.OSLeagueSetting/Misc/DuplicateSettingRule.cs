namespace OWP.OSLeagueSetting.Misc;

using Extensions;
using Models;

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

        return leaguesByRound.Any() && base.IsDuplicate(model, leaguesByRound);
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

        return leaguesByOfferLine.Any() && base.IsDuplicate(model, leaguesByOfferLine);
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
        var leaguesByBetDelay = GetLeagueSettingByBetDelay(model.BetDelay, leagueSettings);

        return leaguesByBetDelay.Any() && base.IsDuplicate(model, leaguesByBetDelay);
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
        var leaguesByProvider = GetLeagueSettingByProvider(model.Providers, leagueSettings);

        return leaguesByProvider.Any() && base.IsDuplicate(model, leaguesByProvider);
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
        var leaguesByRangeTime = GetLeagueSettingByTimeRange(model.KOFromTime?.TimeOfDay, model.KOToTime?.TimeOfDay, leagueSettings);

        return leaguesByRangeTime.Any() && base.IsDuplicate(model, leaguesByRangeTime);
    }

    private static IList<DuplicateSettingModel> GetLeagueSettingByTimeRange(TimeSpan? koFromTime, TimeSpan? koToTime, IList<DuplicateSettingModel> leagueSettings)
    {
        return leagueSettings.Where(x => IsTimeRangeOverLap(koFromTime, koToTime, x.KOFromTime, x.KOToTime))
            .ToList();
    }

    private static bool IsTimeRangeOverLap(TimeSpan? fromTime, TimeSpan? toTime, TimeSpan? fromTimeDB, TimeSpan? toTimeDB)
    {
        if (IsKoTimeNotExits(fromTime, toTime, fromTimeDB, toTimeDB))
        {
            return true;
        }
        var newKoToTimeModel = GetFormattedKoToTime(fromTime.GetValueOrDefault(), toTime.GetValueOrDefault());
        var newKoToTimeDB = GetFormattedKoToTime(fromTimeDB.GetValueOrDefault(), toTimeDB.GetValueOrDefault());
        return fromTime.GetValueOrDefault() < newKoToTimeDB
               && fromTimeDB.GetValueOrDefault() < newKoToTimeModel;
    }

    private static bool IsKoTimeNotExits(TimeSpan? fromTime, TimeSpan? toTime, TimeSpan? fromTimeDB, TimeSpan? toTimeDB)
        => fromTime is null && toTime is null && fromTimeDB is null && toTimeDB is null;

    private static TimeSpan GetFormattedKoToTime(TimeSpan fromTime, TimeSpan toTime)
    {
        return toTime < fromTime ? toTime + TimeSpan.FromHours(24) : toTime;
    }
}

public class IPTVRule : DuplicateRule
{
    public override bool IsDuplicate(SettingDetail model, IList<DuplicateSettingModel> leagueSettings)
    {
        var leaguesByIpTv = GetLeagueSettingByIPTV(model.IsIptvOnly, leagueSettings);

        return leaguesByIpTv.Any() && base.IsDuplicate(model, leaguesByIpTv);
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
        var leaguesByRemark = GetLeagueSettingByRemark(model.Remark, leagueSettings);

        return leaguesByRemark.Any() && base.IsDuplicate(model, leaguesByRemark);
    }

    private static IList<DuplicateSettingModel> GetLeagueSettingByRemark(string remark,
        IList<DuplicateSettingModel> leagueSettings)
    {
        return leagueSettings.Where(x => x.Remark.Trim()
                .Equals(remark.Trim(), StringComparison.OrdinalIgnoreCase))
                .ToList();
    }
}