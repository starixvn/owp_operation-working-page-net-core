using OWP.AutoAssignment.Utils.XMLExtensions;

namespace OWP.AutoAssignment.Models.InternalApi
{
    [XMLName("Row")]
    public record LAExportQueryInfo
    {
        public int InfoSiteMatchId { get; init; }

        public int LAAssingeeId { get; init; }

        public int Bettype { get; init; }
    }
}