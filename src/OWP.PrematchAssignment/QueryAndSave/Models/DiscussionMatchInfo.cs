namespace OWP.PrematchAssignment.QueryAndSave.Models;

public class DiscussionMatchInfo
{
    public int AdminSiteMatchId { get; set; }

    public string LeagueName { get; set; }

    public int HomeId { get; set; }

    public string HomeName { get; set; }

    public int AwayId { get; set; }
    
    public string AwayName { get; set; }
}