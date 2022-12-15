using Fanex.Data.Repository;

namespace OWP.TaskList.Criteria
{
    public class GetTaskListCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public bool IsGetYesterdayEvent { get; set; }

        public DateTime EventDate { get; set; }

        public override string GetSettingKey() => "OWP_TaskListForTodayGet";

        public override bool IsValid() => UserId > 0;
    }
}
