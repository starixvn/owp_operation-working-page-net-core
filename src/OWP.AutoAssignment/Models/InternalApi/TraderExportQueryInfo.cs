using OWP.AutoAssignment.Utils.XMLExtensions;

namespace OWP.AutoAssignment.Models.InternalApi
{
    [XMLName("Row")]
    public record TraderExportQueryInfo
    {
        public int InfoSiteMatchId { get; init; }

        public int? FTHDPAssingeeId { get; init; }

        public int? FIRSTHHDPAssingeeId { get; init; }

        public int? FTOUAssingeeId { get; init; }

        public int? FIRSTHOUAssingeeId { get; init; }

        public int Bettype { get; init; }
    }
}