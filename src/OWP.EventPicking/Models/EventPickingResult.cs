namespace OWP.EventPicking.Models;

public class EventPickingResult
{
    public List<EventPickingModel> MatchesByDate { get; set; }

    public List<EventPickingModel> FilteredMatches { get; set; }
}
