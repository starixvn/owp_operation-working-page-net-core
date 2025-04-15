using Fanex.NAP.Client.Helpers.Business;
using OWP.Application.Header;
using OWP.Constants;
using OWP.Services;

namespace OWP.Websites.ViewComponents;

public class HeaderViewComponent(INapSession session, HeaderService headerService, PermissionService permissionService)
    : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(string title)
    {
        if (session.UserId == 0)
        {
            return Content("Unauthorized Request");
        }

        var headerViewModel = new HeaderViewModel
        {
            Title = title,
            Header = await headerService.Query(),
            HasDetailInformationPermission =
                permissionService.HasRight(FunctionKeys.DetailsInformation, Constant.Policy.Read),
            HasQuickSearchPermission = permissionService.HasRight(FunctionKeys.QuickSearch, Constant.Policy.Read),
            HasRocketChatPermission = permissionService.HasRight(FunctionKeys.RocketChat, Constant.Policy.Read)
        };

        ViewBag.Title = title;
        return View(headerViewModel);
    }
}