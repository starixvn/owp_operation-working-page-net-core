using Fanex.Data.Repository;

namespace OWP.Application.Message;
public class QueryCriteria : CriteriaBase
{
    public int UserId { get; set; }

    public bool IsWatcher { get; set; }

    public int SelectTop { get; set; }

    public int TotalNotViewMessage { get; set; }

    public override string GetSettingKey() => "OWP_MessageGetByUserId";

    public override bool IsValid() => UserId > 0 && SelectTop >= 0;
}
