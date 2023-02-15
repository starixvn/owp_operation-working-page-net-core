namespace OWP.Models;

public class GetAdminSiteMatchIdRequest
{
    public int OWPMatchId { get; set; }

    public int ParentMatchId { get; set; }

    public int? MinorBettypeId { get; set; }
}