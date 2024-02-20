namespace OWP.Event.LiveGameAssignment.Download.Criteria;

using Fanex.Data.Repository;
using System;

public class MatchGetAllCriteria : CriteriaBase
{
    public DateTime EventDate { get; set; }

    public string MatchStatusIds { get; set; }

    public override bool IsValid() => EventDate > DateTimeOffset.MinValue;

    public override string GetSettingKey() => "OWP_MatchListDetailsGet";
}