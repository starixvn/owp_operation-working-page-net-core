namespace OWP.LiveGameList.Models;

public class EventInformation
{
    public IEnumerable<MatchInfo> MatchInfos { get; set; }

    public Dictionary<int, List<Assignment>> AssignmentsDict { get; set; }
    public Dictionary<int, List<Assignment>> MinorAssignmentsDict { get; set; }
    public Dictionary<int, List<MatchSetting>> MatchSettingsDict { get; set; }
    public Dictionary<int, List<MatchSetting>> MinorMatchSettingsDict { get; set; }
    public Dictionary<int, List<Television>> TelevisionsDict { get; set; }
    public Dictionary<int, List<MatchOffer>> MatchOffersDict { get; set; }
    public Dictionary<int, List<TicketInfo>> TicketInfosDict { get; set; }
}
