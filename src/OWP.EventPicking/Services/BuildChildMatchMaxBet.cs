using OWP.Constants;
using OWP.EventPicking.Models;
using OWP.Extensions;
using OWP.MaxbetSetting.Models;
using OWP.Utils;

namespace OWP.EventPicking.Services;

public interface BuildChildMatchMaxBet
{
    void Build(IEnumerable<EventPickingModel> mainMatchesHasMaxBet, IEnumerable<EventPickingModel> childMatches, Dictionary<decimal, MaxBetSettingModel> maxBetDefault);
}

public class BuildChildMatchMaxBetImp : BuildChildMatchMaxBet
{
    public void Build(IEnumerable<EventPickingModel> mainMatches,
        IEnumerable<EventPickingModel> childMatches,
        Dictionary<decimal, MaxBetSettingModel> maxBetDefault)
    {
        foreach (var child in childMatches)
        {
            var maxBet = GetMaxBetSettingDefault(mainMatches, child.InfoSiteMatchId, maxBetDefault);
            switch (child.MinorBettypeId)
            {
                case Bettype.Corner:
                    child.MaxBetHDP = maxBet.NoOfCorner_FT_HDP.ToOWPMaxBetFormat();
                    child.MaxBetOU = maxBet.NoOfCorner_FT_OU.ToOWPMaxBetFormat();
                    child.MaxBet1HHDP = maxBet.NoOfCorner_1H_HDP.ToOWPMaxBetFormat();
                    child.MaxBet1HOU = maxBet.NoOfCorner_1H_OU.ToOWPMaxBetFormat();
                    break;
                case Bettype.TotalBooking:
                    child.MaxBetHDP = maxBet.TotalBooking_FT_HDP.ToOWPMaxBetFormat();
                    child.MaxBetOU = maxBet.TotalBooking_FT_OU.ToOWPMaxBetFormat();
                    child.MaxBet1HHDP = maxBet.TotalBooking_1H_HDP.ToOWPMaxBetFormat();
                    child.MaxBet1HOU = maxBet.TotalBooking_1H_OU.ToOWPMaxBetFormat();
                    break;
                case Bettype.NextCorner:
                    child.MaxBetHDP = maxBet.NextCorner_FT_HDP.ToOWPMaxBetFormat();
                    break;
                case Bettype.NextBooking:
                    child.MaxBetHDP = maxBet.NextBooking_FT_HDP.ToOWPMaxBetFormat();
                    break;
                default:
                    child.MaxBetHDP = string.Empty;
                    break;
            }
        }
    }

    public static MaxBetSettingModel GetMaxBetSettingDefault(IEnumerable<EventPickingModel> mainMatches, int parentInfositeId, Dictionary<decimal, MaxBetSettingModel> maxBetDefault)
    {
        var parentMaxBet = mainMatches.FirstOrDefault(x => x.InfoSiteMatchId == parentInfositeId);
        if (parentMaxBet != null && parentMaxBet.MaxBetHDPNumber.HasValue)
        {
            return maxBetDefault.ContainsKey((decimal)parentMaxBet.MaxBetHDPNumber)
                ? maxBetDefault.GetValue((decimal)parentMaxBet.MaxBetHDPNumber)
                : maxBetDefault.GetValue((-1000m));
        }

        return null;
    }
}