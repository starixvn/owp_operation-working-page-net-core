using Fanex.Data.Repository;

namespace OWP.Application.DocumentTitle;
public class CountUnreadMessageCriteria : CriteriaBase
{
    public int UserId { get; set; }

    public override string GetSettingKey() => "OWP_CountUnreadMessageByUserId";

    public override bool IsValid() => true;
}
