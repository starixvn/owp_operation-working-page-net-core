using OWP.Extensions;

namespace OWP.AutoAssignment.Models.InternalApi;

[XMLName("Row")]
public class MatchQueryInfo
{
    public int InfoSiteMatchId { get; set; }

    public int BetType { get; set; }
}