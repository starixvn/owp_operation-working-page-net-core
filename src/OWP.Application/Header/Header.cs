namespace OWP.Application.Header;
public class Header
{
    public string Username { get; set; }

    public string LogoutPath { get; set; }

    public string NotificationSettingPath { get; set; }

    public string AccountSettingPath { get; set; }

    public List<Menu> Menus { get; set; }
    public string ChangePasswordPath { get; set; }
}
