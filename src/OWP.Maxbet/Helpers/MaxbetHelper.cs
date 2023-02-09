using OWP.Enums;
using OWP.Maxbet.Constants;
using OWP.Maxbet.Models;

namespace OWP.Maxbet.Helpers;

public static class MaxbetHelper
{
    public static T FindTraderAssignmentByBettype<T>(List<T> assignment, int bettypeId) where T : TraderAssignment
        => assignment.FirstOrDefault(a => a.BettypeId == bettypeId);

    public static T FindSpreadByBettype<T>(List<T> spreads, int bettypeId) where T : SpreadModel
        => spreads.FirstOrDefault(a => a.Bettype == bettypeId);

    public static T FindMaxbetByBettype<T>(List<T> maxbets, int bettypeId) where T : MaxbetModel
        => maxbets.FirstOrDefault(a => a.BettypeId == bettypeId);

    public static void UpdateSpreadPEN(MaxbetSpread spread, decimal newValueHdp, decimal newValueOU)
    {
        spread.Handicap = newValueHdp;
        spread.OverUnder = newValueOU;
    }

    public static void UpdateMaxbetPEN(MaxbetSpread maxbet)
    {
        var hdp = maxbet.Handicap;
        if (hdp.HasValue)
        {
            var hdpPEN = hdp / 2;
            maxbet.Handicap = hdpPEN;
            maxbet.OverUnder = hdpPEN;
        }
    }

    public static void SetDefaultMaxbetInfo(MaxbetInfo maxbetInfo)
    {
        maxbetInfo.Maxbet.FirstHalfHandicap = null;
        maxbetInfo.Maxbet.FirstHalfOverUnder = null;

        maxbetInfo.Spread.FirstHalfHandicap = null;
        maxbetInfo.Spread.FirstHalfOverUnder = null;

        maxbetInfo.Traders.FirstHalfHandicap = null;
        maxbetInfo.Traders.FirstHalfOverUnder = null;

        maxbetInfo.MaxbetCN88.FirstHalfHandicap = null;
        maxbetInfo.MaxbetCN88.FirstHalfOverUnder = null;

        maxbetInfo.NumberOfLines = NumberOfLineHelper.UpdateNumberOfLine();
    }

    public static ExistsValueBettype CanUpdateValueBettype(List<MaxbetModel> maxbet)
    {
        var hasExistsHdp = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.Handicap));
        var hasExistsOu = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.OverUnder));
        var hasExistsFirstHalfHdp = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.FirstHalfHandicap));
        var hasExistsFirstHalfOu = maxbet.Any(s => s.BettypeId.Equals((int)EnumBettype.FirstHalfOverUnder));
        return new ExistsValueBettype
        {
            Hdp = !hasExistsHdp,
            Ou = !hasExistsOu,
            FirstHalfHdp = !hasExistsFirstHalfHdp,
            FirstHalfOu = !hasExistsFirstHalfOu,
        };
    }

    public static MaxbetModel Build(
        int bettypeId,
        decimal? valueBettype,
        decimal? defaultValue)
        => valueBettype.HasValue && valueBettype.Value != MaxBetSetting.FOLLOW_MAIN_MATCH ? new MaxbetModel
        {
            BettypeId = bettypeId,
            Value = valueBettype
        } : new MaxbetModel
        {
            BettypeId = bettypeId,
            Value = defaultValue
        };
}