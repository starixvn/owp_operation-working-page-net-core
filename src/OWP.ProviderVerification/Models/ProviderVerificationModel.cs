
namespace OWP.ProviderVerification.Models;

public class ProviderVerificationModel
{
    public int OwpMatchId { get; set; }

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

    public List<AgentConfirmOption> AgentConfirm { get; set; }

    public string EstNote { get; set; }

    public bool? HasTicket { get; set; }

    public bool? IsRead { get; set; }

    public int ParentMatchId { get; set; }

    public string RowColor => MatchStatus switch
    {
        "Cancel" => "#EEEDDD",
        "Postponed" or "Suspended" => "#DDEFDA",
        _ => "#FFF"
    };

}

public class AgentConfirmOption
{
    public int Value { get; set; }

    public string Label { get; set; }
}

public class AgentConfirmModel
{
    public int OwpMatchId { get; set; }

    public int AgentId { get; set; }

    public string AgentName { get; set; }
}
