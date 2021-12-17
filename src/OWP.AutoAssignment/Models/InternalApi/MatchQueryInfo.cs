namespace OWP.AutoAssignment.Models.InternalApi
{
    using Utils.XMLExtensions;

    [XMLName("Row")]
    public class MatchQueryInfo
    {
        public int InfoSiteMatchId { get; set; }

        public int BetType { get; set; }
    }
}