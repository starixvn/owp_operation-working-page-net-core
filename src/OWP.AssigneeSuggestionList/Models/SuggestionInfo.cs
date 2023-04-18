namespace OWP.AssigneeSuggestionList.Models;

public class SuggestionInfo
{
    public MatchInfo MatchInfo { get; set; }

    public List<WorkingSchedule> FullyAvailable { get; set; }

    public List<WorkingSchedule> PartiallyAvailable { get; set; }

    public List<WorkingSchedule> Busy { get; set; }
}
