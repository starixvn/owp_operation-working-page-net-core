namespace OWP.NotificationSetting.Models;

public class NotificationSettingViewModel
{
    public List<NotificationSettingMapping> Assignees { get; set; }

    public List<NotificationSettingMapping> Watchers { get; set; }

    public bool IsWatcher { get; set; }
}