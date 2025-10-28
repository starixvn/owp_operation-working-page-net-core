using OWP.Constants;
using OWP.Models;

namespace OWP.EventPicking.Api.Services;

public interface BuildChildMatchMaxBet
{
    Task<Dictionary<decimal, MaxBetSettingModel>> GetMaxBetDefaultSetting();

    Task Build(List<EventPickingModel> events);

    Task BuildMBForAddSpecialProduct(List<SpecialProductMaxbetSpread> events, List<EventPickingModel> mainEvents);

    void BuildExport(List<EventPickingModel> mainMatches, List<EventPickingModel> childMatches, Dictionary<decimal, MaxBetSettingModel> maxBetDefaults);
}

public class BuildChildMatchMaxBetImp(MaxbetSettingService maxbetSettingService) : BuildChildMatchMaxBet
{
    public void BuildExport(List<EventPickingModel> mainMatches,
        List<EventPickingModel> childMatches,
        Dictionary<decimal, MaxBetSettingModel> maxBetDefaults)
    {
        foreach (var child in childMatches)
        {
            var maxBetDefault = GetMaxBetSettingDefault(mainMatches, child.InfoSiteMatchId, maxBetDefaults);
            ApplyMaxBetSetting(maxBetDefault, child);
            if (maxBetDefault != null)
            {
                child.IsReferenceLeagueManagement = true;
                switch (child.MinorBettypeId)
                {
                    case Bettype.NoOfCorner:
                        //child.MaxbetHdpExcel = child.MaxbetHdp.HasValue ? maxBetDefault.NoOfCorner_FT_HDP.ToOWPMaxBetFormat();
                        //child.MaxbetOu = maxBetDefault.NoOfCorner_FT_OU.ToOWPMaxBetFormat();
                        child.MaxBet1HHDPExcel = maxBetDefault.NoOfCorner_1H_HDP.ToOWPMaxBetFormat();
                        child.MaxBet1HOUExcel = maxBetDefault.NoOfCorner_1H_OU.ToOWPMaxBetFormat();

                        //child.MaxBetHDP ??= maxBetDefault.NoOfCorner_FT_HDP;
                        //child.MaxBetOU ??= maxBetDefault.NoOfCorner_FT_OU;
                        //child.MaxBet1HHDP ??= maxBetDefault.NoOfCorner_FT_Spread_HDP;
                        //child.MaxBet1HOU ??= maxBetDefault.NoOfCorner_FT_Spread_OU;
                        //child.SpreadHdpExcel = maxBetDefault.NoOfCorner_FT_Spread_HDP;
                        //child.SpreadOu ??= maxBetDefault.NoOfCorner_FT_Spread_OU;
                        break;
                    case Bettype.TotalBooking:
                        //child.MaxBetHDP = maxBetDefault.TotalBooking_FT_HDP.ToOWPMaxBetFormat();
                        //child.MaxBetOU = maxBetDefault.TotalBooking_FT_OU.ToOWPMaxBetFormat();
                        child.MaxBet1HHDPExcel = maxBetDefault.TotalBooking_1H_HDP.ToOWPMaxBetFormat();
                        child.MaxBet1HOUExcel = maxBetDefault.TotalBooking_1H_OU.ToOWPMaxBetFormat();

                        
                        break;
                    case Bettype.NextCorner:
                        //child.MaxBetHDP = maxBetDefault.NextCorner_FT_HDP.ToOWPMaxBetFormat();
                        break;
                    case Bettype.NextBooking:
                        //child.MaxBetHDP = maxBetDefault.NextBooking_FT_HDP.ToOWPMaxBetFormat();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public async Task<Dictionary<decimal, MaxBetSettingModel>> GetMaxBetDefaultSetting()
    {
        var maxBet = await maxbetSettingService.QueryMaxbetSetting();
        return maxBet.ToDictionary(x => x.FT_HDP);
    }

    public async Task Build(List<EventPickingModel> events)
    {
        var maxBetDefaults = await GetMaxBetDefaultSetting();
        var childEvents = events.Where(x => x.IsChildMatch);
        var mainEvents = events.Where(x => !x.IsChildMatch).ToList();
        foreach (var child in childEvents)
        {
            var maxBetDefault = GetMaxBetSettingDefault(mainEvents, child.InfoSiteMatchId, maxBetDefaults);
            ApplyMaxBetSetting(maxBetDefault, child);
        }
    }

    private static void ApplyMaxBetSetting(MaxBetSettingModel maxBetDefault, EventPickingModel child)
    {
        if (maxBetDefault is not null)
        {
            child.IsReferenceLeagueManagement = true;
            switch (child.MinorBettypeId)
            {
                case Bettype.NoOfCorner:
                    ApplyNoOfCornerSettings(child, maxBetDefault);
                    break;
                case Bettype.NextCorner:
                    ApplyNextCornerSettings(child, maxBetDefault);
                    break;
                case Bettype.TotalBooking:
                    ApplyTotalBookingSettings(child, maxBetDefault);
                    break;
                case Bettype.NextBooking:
                    ApplyNextBookingSettings(child, maxBetDefault);
                    break;
                default:
                    child.MaxbetHdp = null;
                    child.IsReferenceLeagueManagement = false;
                    break;
            }
        }
    }

    private static void ApplyNoOfCornerSettings(EventPickingModel child, MaxBetSettingModel maxBetDefault)
    {
        child.MaxbetHdp ??= maxBetDefault.NoOfCorner_FT_HDP;
        child.MaxbetOu ??= maxBetDefault.NoOfCorner_FT_OU;
        child.SpreadHdp ??= maxBetDefault.NoOfCorner_FT_Spread_HDP;
        child.SpreadOu ??= maxBetDefault.NoOfCorner_FT_Spread_OU;
    }

    private static void ApplyNextCornerSettings(EventPickingModel child, MaxBetSettingModel maxBetDefault)
    {
        child.MaxbetHdp ??= maxBetDefault.NextCorner_FT_HDP;
        child.SpreadHdp ??= maxBetDefault.NextCorner_FT_Spread_HDP;
    }

    private static void ApplyTotalBookingSettings(EventPickingModel child, MaxBetSettingModel maxBetDefault)
    {
        child.MaxbetHdp ??= maxBetDefault.TotalBooking_FT_HDP;
        child.MaxbetOu ??= maxBetDefault.TotalBooking_FT_OU;
        child.SpreadHdp ??= maxBetDefault.TotalBooking_FT_Spread_HDP;
        child.SpreadOu ??= maxBetDefault.TotalBooking_FT_Spread_OU;
    }

    private static void ApplyNextBookingSettings(EventPickingModel child, MaxBetSettingModel maxBetDefault)
    {
        child.MaxbetHdp ??= maxBetDefault.NextBooking_FT_HDP;
        child.SpreadHdp ??= maxBetDefault.NextBooking_FT_Spread_HDP;
    }
    public async Task BuildMBForAddSpecialProduct(List<SpecialProductMaxbetSpread> events, List<EventPickingModel> mainEvents)
    {
        var maxBetDefaults = await GetMaxBetDefaultSetting();
        foreach (var child in events)
        {
            var maxBetDefault = GetMaxBetSettingDefault(mainEvents, child.ParentId, maxBetDefaults);
            ApplyMaxBetSettingForSpecialProduct(maxBetDefault, child);
        }
    }

    private static void ApplyMaxBetSettingForSpecialProduct(MaxBetSettingModel maxBetDefault, SpecialProductMaxbetSpread child)
    {
        if (maxBetDefault is not null)
        {
            switch (child.MinorBettypeId)
            {
                case Bettype.NoOfCorner:
                    child.MaxbetHdp = maxBetDefault.NoOfCorner_FT_HDP;
                    child.MaxbetOu = maxBetDefault.NoOfCorner_FT_OU;
                    child.SpreadHdp = maxBetDefault.NoOfCorner_FT_Spread_HDP;
                    child.SpreadOu = maxBetDefault.NoOfCorner_FT_Spread_OU;
                    break;
                case Bettype.NextCorner:
                    child.MaxbetHdp = maxBetDefault.NextCorner_FT_HDP;
                    child.SpreadHdp = maxBetDefault.NextCorner_FT_Spread_HDP;
                    break;
                case Bettype.TotalBooking:
                    child.MaxbetHdp = maxBetDefault.TotalBooking_FT_HDP;
                    child.MaxbetOu = maxBetDefault.TotalBooking_FT_OU;
                    child.SpreadHdp = maxBetDefault.TotalBooking_FT_Spread_HDP;
                    child.SpreadOu = maxBetDefault.TotalBooking_FT_Spread_OU;
                    break;
                case Bettype.NextBooking:
                    child.MaxbetHdp = maxBetDefault.NextBooking_FT_HDP;
                    child.SpreadHdp = maxBetDefault.NextBooking_FT_Spread_HDP;
                    break;
                default:
                    child.MaxbetHdp = null;
                    break;
            }
        }
    }

    public static MaxBetSettingModel GetMaxBetSettingDefault(List<EventPickingModel> mainMatches, int parentInfositeId, Dictionary<decimal, MaxBetSettingModel> maxBetDefault)
    {
        var parentMaxBet = mainMatches.Find(x => x.InfoSiteMatchId == parentInfositeId);
        if (parentMaxBet?.MaxbetHdp == null)
        {
            return null;
        }

        const decimal otherKey = -1000m;
        return maxBetDefault.ContainsKey((decimal)parentMaxBet.MaxbetHdp)
            ? maxBetDefault.GetValue((decimal)parentMaxBet.MaxbetHdp)
            : maxBetDefault.GetValue(otherKey);
    }
}