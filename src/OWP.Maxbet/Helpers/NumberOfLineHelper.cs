using OWP.Maxbet.Models;

namespace OWP.Maxbet.Helpers;

public enum NumberOfLinesEtPen
{
    CreditFirstHalf = 2,
    CreditFt = 2,
    LicenseeFirstHalf = 0,
    LicenseeFt = 0
}

public static class NumberOfLineHelper
{
    public static NumberOfLines UpdateNumberOfLine()
        => new NumberOfLines
        {
            CreditFirstHalf = (int)NumberOfLinesEtPen.CreditFirstHalf,
            CreditFt = (int)NumberOfLinesEtPen.CreditFt,
            LicenseeFirstHalf = (int)NumberOfLinesEtPen.LicenseeFirstHalf,
            LicenseeFt = (int)NumberOfLinesEtPen.LicenseeFt
        };
}