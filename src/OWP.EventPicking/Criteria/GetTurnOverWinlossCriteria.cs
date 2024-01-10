namespace OWP.EventPicking.Criteria;

public class GetTurnOverWinLossCriteria
{
    public static string SportType => "soccer";

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public IList<string> LeagueName { get; set; }
}