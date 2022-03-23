using OWP.Maxbet.Models;

namespace OWP.Maxbet.Helpers
{
  public static class MaxbetHelper
  {
    private const decimal MAXBET_HDP = 70000;

    private const decimal MAXBET_ET = 35000;

    public static T FindTraderAssignmentByBettype<T>(List<T> assignment, int bettypeId) where T : TraderAssignment
        => assignment.FirstOrDefault(a => a.BettypeId == bettypeId);

    public static T FindSpreadByBettype<T>(List<T> spreads, int bettypeId) where T : SpreadModel
        => spreads.FirstOrDefault(a => a.Bettype == bettypeId);

    public static T FindMaxbetByBettype<T>(List<T> maxbets, int bettypeId) where T : MaxbetModel
        => maxbets.FirstOrDefault(a => a.BettypeId == bettypeId);

    public static void UpdateMaxbetPEN(MaxbetSpread maxbet)
    {
      var hdp = maxbet.Handicap;
      if (hdp.HasValue)
      {
        var hdpPEN = hdp / 2;
        maxbet.Handicap = hdpPEN;
        maxbet.Handicap = hdpPEN;
      }
    }

    internal static void UpdateSpreadPEN(MaxbetSpread spread, decimal hdpNewValue, decimal ouNewValue)
    {
      spread.Handicap = hdpNewValue;
      spread.OverUnder = ouNewValue;
    }

    internal static void Reset1HSpreadPEN(MaxbetSpread spread)
    {
      spread.FirstHalfHandicap = null;
      spread.FirstHalfOverUnder = null;
    }


    internal static void UpdateMaxbetET(MaxbetSpread maxbet)
    {
      if (maxbet.Handicap == MAXBET_HDP)
      {
        maxbet.Handicap = MAXBET_ET;
        maxbet.OverUnder = MAXBET_ET;
      }
    }

    internal static void Reset1HMaxbetETPEN(MaxbetSpread maxbet)
    {
      maxbet.FirstHalfHandicap = null;
      maxbet.FirstHalfHandicap = null;
    }

    public static void Reset1HTraderETPEN(TraderAssignmentSender trader)
    {
      trader.FirstHalfHandicap = null;
      trader.FirstHalfOverUnder = null;
    }

    internal static void SetDefaultMaxbetInfo(MaxbetInfo maxbetInfo)
    {
      maxbetInfo.Maxbet.FirstHalfHandicap = null;
      maxbetInfo.Maxbet.FirstHalfOverUnder = null;

      maxbetInfo.Spread.FirstHalfHandicap = null;
      maxbetInfo.Spread.FirstHalfOverUnder = null;

      maxbetInfo.Traders.FirstHalfHandicap = null;
      maxbetInfo.Traders.FirstHalfOverUnder = null;

      NumberOfLineHelper.UpdateNumberOfLine(maxbetInfo.NumberOfLines);
    }
  }
}