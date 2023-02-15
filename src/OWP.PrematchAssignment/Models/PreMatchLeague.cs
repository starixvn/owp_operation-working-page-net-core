namespace OWP.PreMatchAssignment.Models;

public class PreMatchLeague
{
    public int No { get; set; }

    public int PreMatchLeagueId { get; set; }

    public int LeagueId { get; set; }

    public string LeagueName { get; set; }

    public int MatchCount { get; set; }

    public string HDP { get; set; }

    public string OU { get; set; }

    public string HDPTakeOver { get; set; }

    public string OUTakeOver { get; set; }

    public string HDPNickname { get; set; }

    public string OUNickname { get; set; }

    public string HDPTakeOverNickname { get; set; }

    public string OUTakeOverNickname { get; set; }

    public string OtherHDP { get; set; }

    public string OtherOU { get; set; }

    public string OtherHDPTakeOver { get; set; }

    public string OtherOUTakeOver { get; set; }

    public string OtherHDPNickname { get; set; }

    public string OtherOUNickname { get; set; }

    public string OtherHDPTakeOverNickname { get; set; }

    public string OtherOUTakeOverNickname { get; set; }

    public string HDPHint { get; set; }

    public string OUHint { get; set; }

    public string HDPTakeOverHint { get; set; }

    public string OUTakeOverHint { get; set; }

    public string OtherHDPHint { get; set; }

    public string OtherOUHint { get; set; }

    public string OtherHDPTakeOverHint { get; set; }

    public string OtherOUTakeOverHint { get; set; }

    public bool BlackList { get; set; }

    public string Remark { get; set; }

    public bool HasNotes { get; set; }

    public int? PreMatchNoteId { get; set; }

    public bool IsDeleted { get; set; }
}