namespace OWP.Application.Header;

using System.Collections.Generic;

public class HeaderModel
{
    public string Username { get; set; }

    public string LogoutPath { get; set; }

    public string NotificationSetingPath { get; set; }

    public List<Menu> Menus { get; set; }
}
