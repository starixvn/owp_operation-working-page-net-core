

using Fanex.Data.Repository;

namespace OWP.MessageManagement.Criteria;
public class MessageBoxUpdateCriteria : CriteriaBase, INonQueryCommand
{
    public int UserId { get; set; }

    public int MessageId { get; set; }

    public override string GetSettingKey() => "OWP_MessageUpdateReadStatus";

    public override bool IsValid() => true;
}
