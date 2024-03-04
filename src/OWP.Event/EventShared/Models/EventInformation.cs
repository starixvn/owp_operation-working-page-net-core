namespace OWP.Event.EventShared.Models;

public class EventInformation
{
    public IEnumerable<MatchInfo> MatchInfo { get; set; }

    public Dictionary<int, List<Assignment>> AssignmentsPair { get; set; }
    public Dictionary<int, List<Assignment>> MinorAssignmentsPair { get; set; }
    public Dictionary<int, List<MatchSetting>> MatchSettingsPair { get; set; }
    public Dictionary<int, List<MatchSetting>> MinorMatchSettingsPair { get; set; }
    public Dictionary<int, List<Television>> TelevisionsPair { get; set; }
    public Dictionary<int, List<MatchOffer>> MatchOffersPair { get; set; }

    public IList<HasTicketModel> HasTicketModel { get; set; }
}
