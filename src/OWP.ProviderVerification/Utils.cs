namespace OWP.ProviderVerification;
internal static class Utils
{
    public static string BuildProviderVerificationKey(string sessionId)
    {
        return $"{Const.Const.KEY_PROVIDER_VERIFICATION}-{sessionId}";
    }

    public static string BuildProviderVerificationDifferenceKey(string sessionId)
    {
        return $"{Const.Const.KEY_CACHE_PROVIDER_VERIFICATION_DIFFERENCE}-{sessionId}";
    }
}
