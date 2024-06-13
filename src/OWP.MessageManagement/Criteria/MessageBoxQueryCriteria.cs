
namespace OWP.MessageManagement.Criteria;
using Fanex.Data.Repository;

public class MessageBoxQueryCriteria : CriteriaBase
{ 
    public string Keyword { get; set; }

    public int UserId { get; set; }

    public int PageSize { get; set; }

    public int PageNumber { get; set; }

    public bool? IsWatcher { get; set; }

    public override string GetSettingKey() => "OWP_MessageGetByUserIdWithPaging";

    public override bool IsValid() => true;
}

