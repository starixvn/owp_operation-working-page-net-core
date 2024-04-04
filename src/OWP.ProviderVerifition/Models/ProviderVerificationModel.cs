
namespace OWP.ProviderVerification.Models;

public class ProviderVerificationModel
{
    public string MatchNo { get; set; }

    public string MatchStatus { get; set; }

    public string KoTime { get; set; }

    public string League { get; set; }

    public string Home { get; set; }

    public string Away { get; set; }

    public string OtherProvider { get; set; }

    public string Provider { get; set; }

    public string Agent { get; set; }

    public string Remark { get; set; }

    public string RemarkBy { get; set; }

    public string AgentConfirm { get; set; }

    public string ESTNote { get; set; }

    public bool? HasTicket { get; set; }

    public bool? IsRead { get; set; }

    public string RowColor => MatchStatus switch
    {
        "Cancel" => "#EEEDDD",
        "Postponed" or "Suspended" => "#DDEFDA",
        _ => "#FFF"
    };

}
