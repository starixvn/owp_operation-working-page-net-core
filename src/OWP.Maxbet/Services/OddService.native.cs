using OWP.Maxbet.Constants;
using OWP.Maxbet.Helpers;
using OWP.Maxbet.Models;
using KeyValuePair = OWP.Maxbet.Models.KeyValuePair;

namespace OWP.Maxbet.Services;
public interface OddService
{
    Task BuildOdd(MaxbetInfo maxbetInfo, int owpMatchId);
}

public class OddServiceImp : OddService
{
    private readonly MaxbetRepository maxbetRepository;

    public OddServiceImp(
        MaxbetRepository maxbetRepository)
    {
        this.maxbetRepository = maxbetRepository;
    }

    public async Task BuildOdd(MaxbetInfo maxbetInfo, int owpMatchId)
    {
        var odd = (await maxbetRepository.GetOddByOwpId(owpMatchId)).ToList();
        BuildOddHandicap(maxbetInfo, odd);
        BuildOddOverUnder(maxbetInfo, odd);
    }

    private void BuildOddHandicap(MaxbetInfo maxbetInfo, IList<KeyValuePair> oddInfo)
    {
        maxbetInfo.OddsHandicap = new OddHandicapInfo
        {
            OddsHome = MaxbetHelper.FindOddByKey(oddInfo, OddConst.ODDS_HOME)?.Value,
            OddsAway = MaxbetHelper.FindOddByKey(oddInfo, OddConst.ODDS_AWAY)?.Value,
            Hdp1 = MaxbetHelper.FindOddByKey(oddInfo, OddConst.ODDS_HANDICAP_HDP1)?.Value,
            Hdp2 = MaxbetHelper.FindOddByKey(oddInfo, OddConst.ODDS_HANDICAP_HDP2)?.Value
        };
    }

    private void BuildOddOverUnder(MaxbetInfo maxbetInfo, IList<KeyValuePair> oddInfo)
    {
        maxbetInfo.OddsOverUnder = new OddOverUnderInfo
        {
            OddsOver = MaxbetHelper.FindOddByKey(oddInfo, OddConst.ODDS_OVER)?.Value,
            OddsUnder = MaxbetHelper.FindOddByKey(oddInfo, OddConst.ODDS_UNDER)?.Value,
            Hdp1 = MaxbetHelper.FindOddByKey(oddInfo, OddConst.ODDS_OU_HDP1)?.Value,
            Hdp2 = MaxbetHelper.FindOddByKey(oddInfo, OddConst.ODDS_OU_HDP2)?.Value
        };
    }
}
