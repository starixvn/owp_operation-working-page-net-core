using OWP.Extensions;

namespace OWP.AssigneeSuggestionList.Helper;
public static class SingleMatchHelper
{
    public static bool IsSingleMatch(decimal? maxbetHdp, string leagueName)
     => maxbetHdp > 12_000 || IsSingleMatchSpecialCase( leagueName);

    private static bool IsSingleMatchSpecialCase( string leagueName)
    {
        switch ( leagueName)
        {
            case var _ when leagueName.IsContains("Fantasy "):
            case var _ when leagueName.IsContains("FUTSAL")
                            && !leagueName.IsContains("SABA "):
                return true;
            default:
                return false;
        }
    }

}
