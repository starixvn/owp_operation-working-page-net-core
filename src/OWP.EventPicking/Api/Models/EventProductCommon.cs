
namespace OWP.EventPicking.Api.Models;

public interface EventProductCommon
{
    string LeagueName { get; set; }

    string HomeName { get; set; }

    string AwayName { get; set; }

    int? MinorBettypeId { get; }

    int InfoSiteMatchId { get; set; }

    decimal? MaxBetHDPNumber { get; set; }

    decimal? MaxbetHdp { get; set; }

    bool IsSaba { get; }

    public DateTime EventDateTime { get; set; }

    public string MatchNo { get; set; }

    public bool IsNoOffer { get; set; }

    public bool IsTestMatch { get; set; }

}
