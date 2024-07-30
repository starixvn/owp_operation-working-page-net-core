namespace OWP.EventPicking.Api.Criteria;

public class GetTurnOverWinLossCriteria
{
    public string SportType { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public IList<string> LeagueName { get; set; }
}