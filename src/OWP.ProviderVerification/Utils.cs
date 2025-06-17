namespace OWP.ProviderVerification;
public static class Utils
{
    public static string BuildProviderVerificationKey(string sessionId)
    {
        return $"{Const.Const.KEY_PROVIDER_VERIFICATION}-{sessionId}";
    }

    public static string BuildProviderVerificationDifferenceKey(string sessionId)
    {
        return $"{Const.Const.KEY_CACHE_PROVIDER_VERIFICATION_DIFFERENCE}-{sessionId}";
    }

    public static string BuildFileAgentUnmapKey(string sessionId)
    {
        return $"{Const.Const.KEY_CACHE_FILE_AGENT_UNMAP}-{sessionId}";
    }
}
