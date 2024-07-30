namespace OWP.NotificationSetting.Models;

public class NotificationSettingUpdateModel
{
    public int UserId { get; set; }

    public int NotificationSettingId { get; set; }

    public string Value { get; set; }
}