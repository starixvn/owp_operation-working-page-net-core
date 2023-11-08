using Fanex.Data.Repository;

namespace OWP.Event.LiveGameList.Criteria;

public class GetEventsCriteria : CriteriaBase
{
    public DateTime EventDate { get; set; }

    public string MatchStatusIds { get; set; }

    public bool ShowAll { get; set; }

    public int ActorId { get; set; }

    public override bool IsValid()
    {
        return EventDate > DateTimeOffset.MinValue;
    }

    public override string GetSettingKey() => "OWP_LiveGameList_QueryEvents";
}