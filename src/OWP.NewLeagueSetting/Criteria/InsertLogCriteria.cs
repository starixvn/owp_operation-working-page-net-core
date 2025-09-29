using Fanex.Data.Repository;

namespace OWP.NewLeagueSetting.Criteria;
public  class InsertLogCriteria: NonQueryCommand
{
    public int SettingId { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Action { get; set; }
    public string Content { get; set; }

    public override string GetSettingKey() => "OWP_LeagueManagement_InsertLog";

    public override bool IsValid() => true;
}
