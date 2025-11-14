namespace OWP.PrematchAssignment.QueryAndSave.Models;

public class DiscussionInfo
{
    public int DiscussionId { get; set; }

    public int AdminSiteMatchId { get; set; }

    public int BetType { get; set; }

    public string RCDiscussionGroupId { get; set; }
    
    public string BetOn { get; set; }

    public string LeagueName { get; set; }

    public int HomeId { get; set; }

    public string HomeName { get; set; }

    public int AwayId { get; set; }

    public string AwayName { get; set; }

    public string GhostName { get; set; }

    public string GhostDetail { get; set; }

    public decimal HdpOu {get; set; }

    public string Odds { get; set; }

    public string Note { get; set; }

    public string Remark { get; set; }

    public int CreatedBy { get; set; }
}