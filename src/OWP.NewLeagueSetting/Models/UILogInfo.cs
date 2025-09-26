namespace OWP.NewLeagueSetting.Models;
public class UILogInfo
{
    public int LogId { get; set; }

    public int SettingId { get; set; }

    public DateTime CreatedDate { get; set; }

    public string ModifiedBy { get; set; }

   public IList<LogDetail> Details { get; set; }
}

public class LogDetail
{
    public string Field { get; set; }

    public string OldValue { get; set; }

    public string NewValue { get; set; }
}
