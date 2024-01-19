namespace OWP.Event.LiveGameAssignment.Download.Criteria.TaskListToday;

using System;
using Fanex.Data.Repository;

public class GetTaskListTodayCriteria : CriteriaBase
{
    public int UserId { get; set; }

    public bool IsGetYesterdayEvent { get; set; }

    public DateTime EventDate { get; set; }

    public override string GetSettingKey() => "OWP_TaskListForTodayGet";

    public override bool IsValid() => UserId > 0;
}