
using Epplus.Mapper.Annotations;

namespace OWP.ProviderVerification.Models;

public class EventModel
{
    public int ?OWPMatchId { get; set; }

    [Cell("A2")]
    public string MatchNo { get; set; }

    [Cell("B2")]
    public string MatchStatus { get; set; }

    [Cell("C2")]
    public DateTime ?EventDateTime { get; set; }

    [Cell("D2")]
    public string KoTime { get; set; }

    [Cell("G2")]
    public string League { get; set; }

    [Cell("E2")]
    public string Home { get; set; }

    [Cell("F2")]
    public string Away { get; set; }

    [Cell("K2")]
    public string Remark { get; set; }

    [Cell("L2")]
    public string RemarkBy { get; set; }

    [Cell("H2")]
    public string LstBookProvider { get; set; }

    [Cell("I2")]
    public string InHouseProviderId { get; set; }

    public string ProviderMatchId { get; set; }

    public string Provider { get; set; }
}

public class ProviderVerificationModel : EventModel
{
    public List<AgentConfirmOption> AgentConfirm { get; set; }

    public List<ProviderMatchModel> ProviderMatch { get; set; }


    public string RowColor => MatchStatus switch
    {
        "Cancel" => "#FDFBDE",
        "Postponed" or "Suspended" => "#DDEFDA",
        _ => "#FFF"
    };

    public bool? HasTicket { get; set; }

    public bool? IsRead { get; set; }

}
