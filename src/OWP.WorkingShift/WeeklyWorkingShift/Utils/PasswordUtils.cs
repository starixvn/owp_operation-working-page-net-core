namespace OWP.WorkingShift.WeeklyWorkingShift.Utils;

public static class PasswordUtils
{
    private const string EXCEL_PASSWORD = "8888";

    private const string EXCEL_PASSWORD_IFRAME = "sms888";

    public static string BuildPassword(bool isFrame)
        => isFrame
            ? EXCEL_PASSWORD_IFRAME
            : EXCEL_PASSWORD;
}