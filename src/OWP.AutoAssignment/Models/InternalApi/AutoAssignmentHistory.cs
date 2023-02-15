namespace OWP.AutoAssignment.Models.InternalApi;

public class AutoAssignmentHistory
{
    public string RequestId { get; set; }

    public int InfoSiteMatchId { get; set; }

    public int? LeagueSettingId { get; set; }

    public decimal? MaxBetFTHDP { get; set; }

    public decimal? MaxBetNoOfCorner { get; set; }

    public decimal? MaxBetNextCorner { get; set; }
}