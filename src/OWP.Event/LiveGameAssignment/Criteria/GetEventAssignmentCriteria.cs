namespace OWP.Event.LiveGameAssignment.Criteria;

using System;
using Fanex.Data.Repository;

public class GetEventAssignmentCriteria : CriteriaBase
{
    public DateTime EventDate { get; set; }

    public string MatchStatusIds { get; set; }

    public override string GetSettingKey() => "OWP_LiveGameAssignment_Query";

    public override bool IsValid() => true;
}