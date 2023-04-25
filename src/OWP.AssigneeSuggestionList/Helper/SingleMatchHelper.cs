using OWP.Extensions;

namespace OWP.AssigneeSuggestionList.Helper;
public static class SingleMatchHelper
{
    public static bool IsSingleMatch(decimal maxbetHdp, decimal spreadHdp, string leagueName)
     => maxbetHdp > 12_000 || IsSingleMatchSpecialCase(maxbetHdp, spreadHdp, leagueName);

    private static bool IsSingleMatchSpecialCase(decimal maxbetHdp, decimal spreadHdp, string leagueName)
    {
        switch (maxbetHdp, spreadHdp, leagueName)
        {
            case var _ when leagueName.IsContains("Fantasy "):
            case var _ when leagueName.IsContains("FUTSAL")
                            && !leagueName.IsContains("SABA "):
            case (10_000, 0.08m or 0.10m, _):
            case (12_000, 0.08m or 0.10m, _):
                return true;
            default:
                return false;
        }
    }

}
