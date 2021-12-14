namespace OWP.AutoAssignment.Models.InternalApi
{
    public record LAExportQueried
    {
        public int InfoSiteMatchId { get; init; }

        public int LAAssingeeId { get; init; }

        public int Bettype { get; init; }
    }
}