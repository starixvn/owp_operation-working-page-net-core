namespace OWP.NewLeagueSetting.Models;
public class DbLogInfo
{
    public int LogId { get; set; }

    public int SettingId { get; set; }

    public DateTime CreatedDate { get; set; }

    public string ModifiedBy { get; set; }

    public string Action { get; set; }

    public string Content { get; set; }


}
