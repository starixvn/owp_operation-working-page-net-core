using OWP.PreMatchAssignment.Models;

namespace OWP.PrematchAssignment.Models;

public class NightShiftLeagueAssignment : LeagueAssignment
{
    public string HDPHint { get; set; }

    public string OUHint { get; set; }

    public string HDPTakeOverHint { get; set; }

    public string OUTakeOverHint { get; set; }

    public string OtherHDPHint { get; set; }

    public string OtherOUHint { get; set; }

    public string OtherHDPTakeOverHint { get; set; }

    public string OtherOUTakeOverHint { get; set; }
}
