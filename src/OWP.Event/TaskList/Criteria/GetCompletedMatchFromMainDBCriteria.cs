using Fanex.Data.Repository;

namespace OWP.Event.TaskList.Criteria;

public class GetCompletedMatchFromMainDBCriteria : CriteriaBase
{
    public string MatchInfoJson { get; set; }

    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_TaskList_GetCompletedMatchFromMainDB";
}