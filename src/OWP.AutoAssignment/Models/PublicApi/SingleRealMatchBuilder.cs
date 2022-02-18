namespace OWP.AutoAssignment.Models.PublicApi
{
    public record SingleRealMatchBuilder
    {
        public RealMatchCommonInfo Match { get; init; }

        public MaxBetOfFTHDP MaxBetHdp { get; init; }

        public MaxBetCorner MaxBetCorners { get; init; }
    }
}