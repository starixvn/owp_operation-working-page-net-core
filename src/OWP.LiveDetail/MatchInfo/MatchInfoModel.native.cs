using Newtonsoft.Json;
using OWP.Utils;

namespace OWP.LiveDetail.MatchInfo;

public class MatchInfoModel
{
    public IEnumerable<Television.Television> TVs { get; set; }

    public string Channel { get; set; }

    [JsonIgnore]
    public string IpTv { get; set; }

    public string WB { get; set; }

    public IEnumerable<OtherProvider.OtherProvider> OtherProviders { get; set; }

    public string OtherProvider => OtherProviders.Any() ? string.Join(", ", OtherProviders.Select(x => x.OtherProviderName)) : string.Empty;

    public IEnumerable<Provider.Provider> Providers { get; set; }

    public string Provider => Providers.Any() ? string.Join(", ", Providers.Select(x => x.ProviderName)) : string.Empty;

    public IEnumerable<Agent.Agent> Agents { get; set; }

    public IEnumerable<MatchOfferModel> MatchOffers { get; set; }

    public string Phone { get; set; }

    public string Remark { get; set; }

    public string Market { get; set; }

    public bool IsSelected { get; set; }

    public IEnumerable<MatchStatusModel> MatchStatusList { get; set; }

    public int? NoOfHDPId { get; set; }

    public IEnumerable<RefResourceModel> NoOfHDPs { get; set; }

    public bool IsSoloMatch { get; set; }

    public string Round { get; set; }

    public string Division { get; set; }

    public string Venue { get; set; }

    public TimeSpan? OtherTime { get; set; }

    public string HomeName { get; set; }

    public string AwayName { get; set; }

    public string LeagueName { get; set; }

    public string MatchNo { get; set; }

    public DateTime EventDate { get; set; }

    public string EventDateDisplay => EventDate.ToOWPDateFormat();

    public DateTime KickOffTime { get; set; }

    public string KickOffTimeDisplay => KickOffTime.ToShortTimeString();

    public long? InfoSiteMatchId { get; set; }
    public long? AdminSiteId { get; set; }

    public string Website1 { get; set; }

    public string Website2 { get; set; }

    public string Website3 { get; set; }

    public string Website4 { get; set; }

    public int? ParentMatchId { get; set; }

    public int? MinorBettypeId { get; set; }

    public string ParentMatchStatus { get; set; }

    public int OWPMatchId { get; set; }
}