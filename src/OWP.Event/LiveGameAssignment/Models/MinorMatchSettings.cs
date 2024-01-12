namespace OWP.Event.LiveGameAssignment.Models;

using System.Collections.Generic;

public class MinorMatchSettings
{
    public int MinorBettypeId { get; set; }

    public List<MatchSettingModel> MatchSettings { get; set; }
}
