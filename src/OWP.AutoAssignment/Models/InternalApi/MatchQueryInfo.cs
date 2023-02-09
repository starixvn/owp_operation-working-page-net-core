namespace OWP.AutoAssignment.Models.InternalApi;

using Extensions;

[XMLName("Row")]
public class MatchQueryInfo
{
    public int InfoSiteMatchId { get; set; }

    public int BetType { get; set; }
}