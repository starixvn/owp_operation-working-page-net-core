using OWP.Constants;
using OWP.EventPicking.Models;
using OWP.Extensions;
using OWP.MaxbetSetting.Models;

namespace OWP.EventPicking.Services;

public interface BuildMatchOfferByLiveSetting
{
    void Build(List<EventPickingModel> mainMatches, List<EventPickingModel> childMatches, Dictionary<decimal, MaxBetSettingModel> settingDefault);

    void BuildForMainMatch(List<EventPickingModel> mainMatches, Dictionary<decimal, MaxBetSettingModel> settingDefault);
}

public class BuildMatchOfferByLiveSettingImp : BuildMatchOfferByLiveSetting
{
    public void Build(List<EventPickingModel> mainMatches, List<EventPickingModel> childMatches, Dictionary<decimal, MaxBetSettingModel> settingDefault)
    {
        var hasMaxBetMainMatches = mainMatches.Where(x => x.MaxBetHDPNumber.HasValue).ToList();
        BuildMatchOfferMainMatches(hasMaxBetMainMatches, settingDefault);
        BuildMatchOfferChildMatches(hasMaxBetMainMatches, childMatches, settingDefault);
    }

    public void BuildForMainMatch(List<EventPickingModel> mainMatches, Dictionary<decimal, MaxBetSettingModel> settingDefault)
    {
        var hasMaxBetMainMatches = mainMatches.Where(x => x.MaxBetHDPNumber.HasValue).ToList();
        BuildMatchOfferMainMatches(hasMaxBetMainMatches, settingDefault);
    }

    private static void BuildMatchOfferMainMatches(List<EventPickingModel> mainMatches, Dictionary<decimal, MaxBetSettingModel> settingDefault)
    {
        mainMatches.ForEach(x =>
        {
            x.MatchOffer = string.IsNullOrEmpty(x.MatchOffer)
                ? GetMatchOffer(settingDefault, x, x.MaxBetHDPNumber)
                : x.MatchOffer;
        });
    }

    private static void BuildMatchOfferChildMatches(List<EventPickingModel> mainMatches, List<EventPickingModel> childMatches, Dictionary<decimal, MaxBetSettingModel> settingDefault)
    {
        foreach (var child in childMatches)
        {
            var parentMatch = mainMatches.FirstOrDefault(x => x.InfoSiteMatchId == child.InfoSiteMatchId);
            if (parentMatch != null)
            {
                child.MatchOffer = GetMatchOffer(settingDefault, child, parentMatch.MaxBetHDPNumber);
            }
        }
    }

    private static string GetMatchOffer(
        Dictionary<decimal, MaxBetSettingModel> liveSettingsDefault,
        EventPickingModel match,
        decimal? maxBetHDP)
    {
        var liveSettingDefault = liveSettingsDefault.ContainsKey(maxBetHDP ?? 0)
            ? liveSettingsDefault.GetValue(maxBetHDP ?? 0)
            : liveSettingsDefault.GetValue(-1000m);
        var isFantasyMatch = match.LeagueName.IsContains("FANTASY");

        return (match.MinorBettypeId, isFantasyMatch) switch
        {
            (Bettype.Corner, _) => liveSettingDefault.SecondAccept_Corner,
            (Bettype.NextCorner, _) => liveSettingDefault.SecondAccept_Corner,
            (Bettype.TotalBooking, _) => liveSettingDefault.SecondAccept_TotalBooking,
            (null, true) => liveSettingDefault.SecondAccept_Fantasy, // fantasy match
            (null, false) => liveSettingDefault.SecondAccept_Main, // main match
            _ => null
        };
    }
}