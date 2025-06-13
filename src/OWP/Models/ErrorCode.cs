namespace OWP.Models;

public enum ErrorCode
{
    Error = -1,
    Ok = 0,
    WarningToContinue = 1,
    PROVIDER_IS_REQUIRED = 10001,
    PROVIDER_MATCH_ID_IS_REQUIRED = 1002,
    PROVIDER_TEMPLATE_REQUIRED = 1003
}