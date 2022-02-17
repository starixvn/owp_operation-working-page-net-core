using Fanex.Data.Repository;
using System;

namespace OWP.TicketCenter.Criteria
{
  public class GetMessageUnreadByMultipleUserIdCriteria : CriteriaBase
  {
    public string UserIds { get; set; }

    public override string GetSettingKey() => "OWP_CountMessageUnreadByMultipleUserId";

    public override bool IsValid() => true;
  }
}
