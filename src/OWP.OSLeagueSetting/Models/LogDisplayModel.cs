namespace OWP.OSLeagueSetting.Models;

public class LogDisplayModel
{
    public int Action { get; set; }
    public LogDataLeagueSetting OldValue { get; set; }
    public LogDataLeagueSetting NewValue { get; set; }
    public string ModifyDate { get; set; }
    public string ModifyBy { get; set; }
}