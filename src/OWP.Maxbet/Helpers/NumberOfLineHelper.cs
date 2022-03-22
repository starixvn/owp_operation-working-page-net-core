using OWP.Maxbet.Models;

namespace OWP.Maxbet.Helpers
{
  public enum NumberOfLinesETPEN
  {
    CreditFirstHalf = 2,
    CreditFt = 2,
    LicenseeFirstHalf = 0,
    LicenseeFt = 0
  }
  public static class NumberOfLineHelper
  {
    public static void UpdateNumberOfLine(NumberOfLines numberOfLines)
    {
      numberOfLines.CreditFirstHalf = (int)NumberOfLinesETPEN.CreditFirstHalf;
      numberOfLines.CreditFt = (int)NumberOfLinesETPEN.CreditFt;
      numberOfLines.LicenseeFirstHalf = (int)NumberOfLinesETPEN.LicenseeFirstHalf;
      numberOfLines.LicenseeFt = (int)NumberOfLinesETPEN.LicenseeFt;
    }
  }
}