using Fanex.Data.Repository;
using System;
namespace OWP.TicketCenter.Criteria
{
  public class CountUnreadMessageCriteria : CriteriaBase
  {
    public int UserId { get; set; }

    public override string GetSettingKey() => "OWP_CountUnreadMessageByUserId";

    public override bool IsValid() => true;
  }
}
