

namespace OWP.AutoAssignment.Models.PublicApi
{
    public record MultipleRealMatchBuilder
    {
        public IList<RealMatchCommonInfo> RealMatches { get; init; }

        public IList<MaxBetOfFTHDP> MaxBetHdpInfo { get; init; }

        public IList<MaxBetCorner> MaxBetCorners { get; init; }
    }
}