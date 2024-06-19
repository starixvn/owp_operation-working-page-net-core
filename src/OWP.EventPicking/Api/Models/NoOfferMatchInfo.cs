namespace OWP.EventPicking.Api.Models;

public class NoOfferMatchInfo
{
    public int InfoSiteMatchId { get; set; }

    public bool IsNoOffer { get; set; }

    public int? ChildMatchId { get; set; }
}