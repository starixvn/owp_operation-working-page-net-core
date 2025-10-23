using OWP.Constants;
using OWP.Models;

namespace OWP.EventPicking.Api.Services;

public interface BuildChildMatchMaxBet
{
    Task<Dictionary<decimal, MaxBetSettingModel>> GetMaxBetDefaultSetting();

    Task Build(List<EventPickingModel> events);

    Task BuildMBForAddSpecialProduct(List<ChildMatchModel> events, List<EventPickingModel> mainEvents);

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
            if (maxBetDefault != null)
            {
                switch (child.MinorBettypeId)
                {
                    case Bettype.NoOfCorner:
                        child.MaxBetHDP = maxBetDefault.NoOfCorner_FT_HDP.ToOWPMaxBetFormat();
                        child.MaxBetOU = maxBetDefault.NoOfCorner_FT_OU.ToOWPMaxBetFormat();
                        child.MaxBet1HHDP = maxBetDefault.NoOfCorner_1H_HDP.ToOWPMaxBetFormat();
                        child.MaxBet1HOU = maxBetDefault.NoOfCorner_1H_OU.ToOWPMaxBetFormat();
                        break;
                    case Bettype.TotalBooking:
                        child.MaxBetHDP = maxBetDefault.TotalBooking_FT_HDP.ToOWPMaxBetFormat();
                        child.MaxBetOU = maxBetDefault.TotalBooking_FT_OU.ToOWPMaxBetFormat();
                        child.MaxBet1HHDP = maxBetDefault.TotalBooking_1H_HDP.ToOWPMaxBetFormat();
                        child.MaxBet1HOU = maxBetDefault.TotalBooking_1H_OU.ToOWPMaxBetFormat();
                        break;
                    case Bettype.NextCorner:
                        child.MaxBetHDP = maxBetDefault.NextCorner_FT_HDP.ToOWPMaxBetFormat();
                        break;
                    case Bettype.NextBooking:
                        child.MaxBetHDP = maxBetDefault.NextBooking_FT_HDP.ToOWPMaxBetFormat();
                        break;
                    default:
                        child.MaxBetHDP = string.Empty;
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
        var maxBetDefaults= await GetMaxBetDefaultSetting();
        var childEvents = events.Where(x => x.MinorBettypeId is null);
        var mainEvents = events.Where(x => x.MinorBettypeId is not null).ToList();
        foreach (var child in childEvents)
        {
            var maxBetDefault = GetMaxBetSettingDefault(mainEvents, child.InfoSiteMatchId, maxBetDefaults);
            if (maxBetDefault is not null)
            {
                switch (child.MinorBettypeId)
                {
                    case Bettype.NoOfCorner:
                        child.MaxbetHdp = maxBetDefault.NoOfCorner_FT_HDP;
                        child.MaxbetOu = maxBetDefault.NoOfCorner_FT_OU;
                        break;
                    case Bettype.NextCorner:
                        child.MaxbetHdp = maxBetDefault.NextCorner_FT_HDP;
                        break;
                    case Bettype.TotalBooking:
                        child.MaxbetHdp = maxBetDefault.TotalBooking_FT_OU;
                        child.MaxbetOu = maxBetDefault.TotalBooking_FT_OU;
                        break;
                    case Bettype.NextBooking:
                        child.MaxbetHdp = maxBetDefault.NextBooking_FT_HDP;
                        break;
                    default:
                        child.MaxBetHDP = string.Empty;
                        break;
                }
            }
        }
    }

    public async Task BuildMBForAddSpecialProduct(List<ChildMatchModel> events, List<EventPickingModel> mainEvents)
    {
        var maxBetDefaults= await GetMaxBetDefaultSetting();
        foreach (var child in events)
        {
            var maxBetDefault = GetMaxBetSettingDefault(mainEvents, child.ParentMatchId, maxBetDefaults);
            if (maxBetDefault is not null)
            {
                switch (child.MinorBettypeId)   
                {
                    case Bettype.NoOfCorner:
                        child.MaxbetHDP = maxBetDefault.NoOfCorner_FT_HDP;
                        child.MaxbetOU = maxBetDefault.NoOfCorner_FT_OU;
                        break;
                    case Bettype.NextCorner:
                        child.MaxbetHDP = maxBetDefault.NextCorner_FT_HDP;
                        break;
                    case Bettype.TotalBooking:
                        child.MaxbetHDP = maxBetDefault.TotalBooking_FT_OU;
                        child.MaxbetOU = maxBetDefault.TotalBooking_FT_OU;
                        break;
                    case Bettype.NextBooking:
                        child.MaxbetHDP = maxBetDefault.NextBooking_FT_HDP;
                        break;
                    default:
                        child.MaxbetHDP = null;
                        break;
                }
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