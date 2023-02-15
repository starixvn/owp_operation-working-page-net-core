namespace OWP.AutoAssignment.Models.PublicApi;

public class MatchQueried
{
    public int InfositeMatchId { get; set; }

    public int? SabaType { get; set; }

    public bool IsSaba => SabaType.HasValue;
}