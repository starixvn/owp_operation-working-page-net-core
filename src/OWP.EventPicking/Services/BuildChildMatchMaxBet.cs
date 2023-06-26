using OWP.Constants;
using OWP.EventPicking.Models;
using OWP.Extensions;
using OWP.MaxbetSetting.Models;
using OWP.Utils;

namespace OWP.EventPicking.Services;

public interface BuildChildMatchMaxBet
{
    void Build(List<EventPickingModel> mainMatches, List<EventPickingModel> childMatches, Dictionary<decimal, MaxBetSettingModel> maxBetDefaults);
}

public class BuildChildMatchMaxBetImp : BuildChildMatchMaxBet
{
    public void Build(List<EventPickingModel> mainMatches,
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
                    case Bettype.Corner:
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

    public static MaxBetSettingModel GetMaxBetSettingDefault(List<EventPickingModel> mainMatches, int parentInfositeId, Dictionary<decimal, MaxBetSettingModel> maxBetDefault)
    {
        var parentMaxBet = mainMatches.FirstOrDefault(x => x.InfoSiteMatchId == parentInfositeId);
        if (parentMaxBet?.MaxBetHDPNumber == null)
        {
            return null;
        }

        const decimal otherKey = -1000m;
        return maxBetDefault.ContainsKey((decimal)parentMaxBet.MaxBetHDPNumber)
            ? maxBetDefault.GetValue((decimal)parentMaxBet.MaxBetHDPNumber)
            : maxBetDefault.GetValue(otherKey);
    }
}