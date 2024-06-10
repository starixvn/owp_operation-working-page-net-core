namespace OWP.Application;
using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Client.Models;
using OWP.Application.Header;
using OWP.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface Service
{
    Task<HeaderModel> Query();
}

public class ServiceImp : Service
{
    private readonly INapSession session;

    public ServiceImp(INapSession session)
    {
        this.session = session;
    }

    public Task<HeaderModel> Query()
    {
        var sessionMenus = session.MenuItemList ?? new List<MenuItem>();
        var menus = GetMenus(sessionMenus);
        var HeaderModel = new HeaderModel
        {
            Username = session.UserName,
            LogoutPath = "/Auth/LogOut",
            NotificationSetingPath = "/UserProfile/NotificationSetting",
            Menus = menus
        };
        return Task.FromResult(HeaderModel);
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