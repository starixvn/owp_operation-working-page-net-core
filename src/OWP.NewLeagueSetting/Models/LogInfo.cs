namespace OWP.NewLeagueSetting.Models;
public class LogInfo
{
    public int LogId { get; set; }

    public int SettingId { get; set; }

    public DateTime CreatedDate { get; set; }

    public string ModifiedBy { get; set; }

    public string Action { get; set; }

    public string Field { get; set; }

    public string OldValue { get; set; }

    public string NewValue { get; set; }


}
