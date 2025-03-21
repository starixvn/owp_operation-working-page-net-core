using Fanex.Data.Repository;

namespace OWP.SimpleDataApi.Criteria.GroupChat;
public class QueryRecipientsCriteria: CriteriaBase
{
    public int ParentId { get; set; }

    public override string GetSettingKey() => "OWP_QuickCreateGroupChat_QueryAllAssigneeByParentId";

    public override bool IsValid() => true;
}
