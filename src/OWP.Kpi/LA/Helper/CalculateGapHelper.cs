namespace OWP.Kpi.LA.Helper;
public static class CalculateGapHelper
{
    public static bool IsGap(DateTime fromDate, DateTime toDate)
        => toDate.Month == 11 && toDate.Year == DateTime.Now.Year &&
           fromDate.Month == 12 && fromDate.Year == DateTime.Now.Year - 2;
}
