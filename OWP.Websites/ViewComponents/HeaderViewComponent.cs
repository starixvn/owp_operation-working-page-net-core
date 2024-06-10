using Fanex.NAP.Client.Helpers.Business;
using OWP.Application.Header;
using OWP.Constants;
using OWP.Services;

namespace OWP.Websites.ViewComponents;

public class HeaderViewComponent : ViewComponent
{
    private readonly INapSession session;
    //private readonly Service headerService;
    private readonly PermissionService permissionService;

    public HeaderViewComponent(INapSession session, PermissionService permissionService)
    {
        this.session = session;
        //this.headerService = headerService;
        this.permissionService = permissionService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string title)
    {
        if (session.UserId == 0)
        {
            return Content("Unauthorized Request");
        }

        var headerViewModel = new HeaderViewModel
        {
            Title = title,
            //Header = await headerService.Query(),
            HasDetailInformationPermission =
                permissionService.HasRight(FunctionKeys.DetailsInformation, Constant.Policy.Read),
            HasQuickSearchPermission = permissionService.HasRight(FunctionKeys.QuickSearch, Constant.Policy.Read)
        };

        ViewBag.Title = title;
        return View(headerViewModel);
    }
}