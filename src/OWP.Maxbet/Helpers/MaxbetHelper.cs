using OWP.Maxbet.Models;

namespace OWP.Maxbet.Helpers
{
  public static class MaxbetHelper
  {
    public static T FindTraderAssignmentByBettype<T>(List<T> assignment, int bettypeId) where T : TraderAssignment
    => assignment.FirstOrDefault(a => a.BettypeId == bettypeId);

    public static T FindSpreadByBettype<T>(List<T> spreads, int bettypeId) where T : SpreadModel
        => spreads.FirstOrDefault(a => a.Bettype == bettypeId);

    public static T FindMaxbetByBettype<T>(List<T> maxbets, int bettypeId) where T : MaxbetModel
        => maxbets.FirstOrDefault(a => a.BettypeId == bettypeId);


    internal static void SetDefaultMaxbetInfo(MaxbetInfo maxbetInfo)
    {
      maxbetInfo.Maxbet.FirstHalfHandicap = null;
      maxbetInfo.Maxbet.FirstHalfOverUnder = null;

      maxbetInfo.Spread.FirstHalfHandicap = null;
      maxbetInfo.Spread.FirstHalfOverUnder = null;

      maxbetInfo.Traders.FirstHalfHandicap = null;
      maxbetInfo.Traders.FirstHalfOverUnder = null;

      maxbetInfo.MaxbetCN88.FirstHalfHandicap = null;
      maxbetInfo.MaxbetCN88.FirstHalfOverUnder = null;

      NumberOfLineHelper.UpdateNumberOfLine(maxbetInfo.NumberOfLines);
      }
    }

    internal static void Reset1HMaxbetETPEN(MaxbetSpread maxbet)
    {
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