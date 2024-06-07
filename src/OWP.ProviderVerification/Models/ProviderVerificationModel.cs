
namespace OWP.ProviderVerification.Models;

public class EventModel
{
    public int OWPMatchId { get; set; }

    public string MatchNo { get; set; }

    public string MatchStatus { get; set; }

    public DateTime EventDateTime { get; set; }

    public string KoTime { get; set; }

    public string League { get; set; }

    public string Home { get; set; }

    public string Away { get; set; }

    public string OtherProvider { get; set; }

    public string Provider { get; set; }

    public string AgentEst { get; set; }

    public string RemarkEst { get; set; }

    public string RemarkBy { get; set; }

    public string EstNote { get; set; }
}

public class ProviderVerificationModel : EventModel
{
    public MatchSetting HandicapSetting { get; set; }

    public MatchSetting OverUnderSetting { get; set; }

    public List<AgentConfirmOption> AgentConfirm { get; set; }

    public string RowColor => MatchStatus switch
    {
        "Cancel" => "#EEEDDD",
        "Postponed" or "Suspended" => "#DDEFDA",
        _ => "#FFF"
    };

    public bool? HasTicket { get; set; }

    public bool? IsRead { get; set; }

}
