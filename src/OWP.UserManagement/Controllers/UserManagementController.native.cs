using Fanex.NAP.Client.Attributes;
using Fanex.NAP.Client.Helpers.Business;
using Fanex.NAP.Client.Helpers.Web;
using Microsoft.AspNetCore.Mvc;
using OWP.Constants;
using OWP.Services;
using OWP.UserManagement.Helpers;
using OWP.UserManagement.Models;
using OWP.UserManagement.Services;

using OWP.Constants;
using OWP.UserManagement.Helpers;
using OWP.UserManagement.Models;
using OWP.UserManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Fanex.NAP.Client.Attributes;
using Microsoft.AspNetCore.Authorization;

namespace OWP.UserManagement.Controllers
{
[Route("api/[controller]")]
[ApiController]
    [Authorize]
public class UserManagementController : ControllerBase
{
    private readonly ExportService exportService;
    private readonly ModelService modelService;
    private readonly Repository repository;
    private readonly INapSession currentSession;
    private readonly AccessCounter accessCounter;
    private readonly RefResourceService refResourceService;

    private readonly UsersService usersService;

    public UserManagementController(
        ExportService exportService,
        ModelService modelService,
        Repository repository, AccessCounter accessCounter,
        UsersService usersService,
        INapSession currentSession, RefResourceService refResourceService)
    {
        this.exportService = exportService;
        this.modelService = modelService;
        this.repository = repository;
        this.currentSession = currentSession;
        this.refResourceService = refResourceService;
        this.accessCounter = accessCounter;
        this.usersService = usersService;
    }


    [NapAuth(FunctionKey = FunctionKeys.UserManagement, Policy = Constant.Policy.Read)]
    public async Task<FilterViewModel> Index()
    {
        var userId = currentSession.UserId;
        var userSectionId = await repository.GetSectionIdByUserId(userId);
        var allResource = await refResourceService.GetAllReferenceResource();
        var permission = modelService.BuildPermissionViewModel();
        var roles = allResource.Where(x => x.Name == ReferenceResourceNames.UserRole);
        var sections = allResource.Where(x => x.Name == ReferenceResourceNames.Section)
            .Where(x => !HiddenSectionConst.HiddenSections.Exists(i => i == x.ResourceId));
        var sectionsByPermission = permission.UserManagementAllSections.IsRead
            ? sections
            : sections.Where(x => x.ResourceId == userSectionId);
        var departments = allResource.Where(x => x.Name == ReferenceResourceNames.Department);
        var categories = allResource.Where(x => x.Name == ReferenceResourceNames.UserCategory);
        var locationCodes = allResource.Where(x => x.Name == ReferenceResourceNames.LocationCode);
        var levels = allResource.Where(x => x.Name == ReferenceResourceNames.UserLevel);
        var userGroups = allResource.Where(x => x.Name == ReferenceResourceNames.UserGroup);
        var languages = allResource.Where(x => x.Name.Equals(ReferenceResourceNames.Language));

        var filter = new FilterViewModel
        {
            PageIndex = PagingConstant.PageIndex,
            PageSize = PagingConstant.PageSize,
            Roles = roles,
            Sections = sectionsByPermission,
            Departments = departments,
            Categories = categories,
            LocationCodes = locationCodes,
            Levels = levels,

            Languages = languages,
            UserGroups = RefResourceHelper.SortUserGroups(userGroups.ToList()),
            Permission = permission
        };
        //var functionName = ConfigurationManager.AppSettings["UserManagement"];
        // await accessCounter.UpdateCounter(functionName);
        return filter;
    }

        [HttpGet("query-options")]
        [NapAuth(FunctionKey = FunctionKeys.UserManagement, Policy = Constant.Policy.Read)]
        public async Task<FilterViewModel> QueryOptions()
        {
            var userId = currentSession.UserId;
            var userSectionId = await repository.GetSectionIdByUserId(userId);
            var allResource = await refResourceService.GetAllReferenceResource();
            var permission = modelService.BuildPermissionViewModel();
            var roles = allResource.Where(x => x.Name == ReferenceResourceNames.UserRole);
            var sections = allResource.Where(x => x.Name == ReferenceResourceNames.Section)
                .Where(x => !HiddenSectionConst.HiddenSections.Exists(i => i == x.ResourceId));
            var sectionsByPermission = permission.UserManagementAllSections.IsRead ? sections
                : sections.Where(x => x.ResourceId == userSectionId);
            var departments = allResource.Where(x => x.Name == ReferenceResourceNames.Department);
            var categories = allResource.Where(x => x.Name == ReferenceResourceNames.UserCategory);
            var locationCodes = allResource.Where(x => x.Name == ReferenceResourceNames.LocationCode);
            var levels = allResource.Where(x => x.Name == ReferenceResourceNames.UserLevel);
            var userGroups = allResource.Where(x => x.Name == ReferenceResourceNames.UserGroup);
            var languages = allResource.Where(x => x.Name.Equals(ReferenceResourceNames.Language));

            return new FilterViewModel
            {
                Roles = roles,
                Sections = sectionsByPermission,
                Departments = departments,
                Categories = categories,
                LocationCodes = locationCodes,
                Levels = levels,

                Languages = languages,
                UserGroups = RefResourceHelper.SortUserGroups(userGroups.ToList()),
                Permission = permission
            };
        }

        [HttpGet("query-permission")]
        [NapAuth(FunctionKey = FunctionKeys.UserManagement, Policy = Constant.Policy.Read)]
        public async Task<UserManagementPermission> QueryUserGroupPermissions()
                => modelService.BuildPermissionViewModel();

    [HttpGet("export-excel")]
    [NapAuth(FunctionKey = FunctionKeys.UserManagement, Policy = Constant.Policy.Read)]
    public async Task<IActionResult> ExportToExcel([FromQuery] QueryModel model)
    {
        const int defaultPageZie = 10000;
        model.PageSize = defaultPageZie;
        model.PageIndex = 1;

        var dataResult = await repository.GetUserPagingAsync(model);

        var exportDate = DateTime.Now;
        byte[] bytes;
        using (var stream = new MemoryStream())
        {
            exportService.ExportToExcel(stream, dataResult.Users, exportDate);
            bytes = stream.ToArray();
        }

        var fileName = $"UserManagement-{exportDate.ToString("yyyyMMdd")}.xlsx";
        return File(bytes, "application/xlsx", fileName);
    }

    [NapAuth(FunctionKey = FunctionKeys.UserManagement, Policy = Constant.Policy.Read)]
    [HttpPost]
    [Route("paging")]
    public async Task<UserManagementViewModel> GetUsers(QueryModel model)
    => await usersService.GetUserPagingAsync(model);
}