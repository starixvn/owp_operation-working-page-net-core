namespace OWP.EventPicking.Api.Models;

public class EventPickingResult
{
    public List<MatchOfferInfo> MatchesByDate { get; set; }

    public List<EventPickingModel> FilteredMatches { get; set; }
}
