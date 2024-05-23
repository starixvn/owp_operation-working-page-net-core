namespace OWP.AccountSetting.QuickMessage.Model;

public class QuickMessage
{
    public int Id { get; set; }
    public string Shortcut { get; set; }
    public string Message { get; set; }
    public int UserId { get; set; }
}