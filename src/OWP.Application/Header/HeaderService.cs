using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Client.Models;
using OWP.Constants;

namespace OWP.Application.Header;

public interface HeaderService
{
    Task<Header> Query();
}

public class HeaderServiceImp : HeaderService
{
    private readonly INapSession session;

    public HeaderServiceImp(INapSession session)
    {
        this.session = session;
    }

    public Task<Header> Query()
    {
        var sessionMenus = session.MenuItemList ?? new List<MenuItem>();
        var menus = GetMenus(sessionMenus);
        var header = new Header
        {
            Username = session.UserName,
            LogoutPath = "/Auth/LogOut",
            NotificationSettingPath = "/UserProfile/NotificationSetting",
            AccountSettingPath = "/UserProfile/AccountSetting",
            Menus = menus
        };
        return Task.FromResult(header);
    }

    private List<Menu> GetMenus(IList<MenuItem> menus)
    {
        return menus.Where(m => m.ParentId == 0)
            .OrderBy(m => m.DisplayOrder)
            .Select(m => new Menu
            {
                Id = m.Id,
                Name = m.Name,
                Path = m.Url,
                IconCssClass = MenuItemExtensions.GetIconCssClass(m.Name),
                Children = GetChildrenMenus(menus, m.Id)
            })
            .ToList();
    }

    private List<Menu> GetChildrenMenus(IEnumerable<MenuItem> menus, int parentMenuId)
    {
        return menus.Where(x => x.ParentId == parentMenuId)
            .OrderBy(m => m.DisplayOrder)
            .Select(m => new Menu
            {
                Id = m.Id,
                Name = m.Name,
                Path = m.Url,
                IconCssClass = MenuItemExtensions.GetIconCssClass(m.Name)
            })
            .ToList();
    }
}