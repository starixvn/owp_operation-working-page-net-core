using Fanex.NAP.Client.Attributes;
using Fanex.NAP.Client.Helpers.Business;
using Fanex.NAP.Client.Helpers.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OWP.Constants;
using OWP.Models;
using OWP.Services;
using OWP.UserManagement.Helpers;
using OWP.UserManagement.Middleware;
using OWP.UserManagement.Models;
using OWP.UserManagement.Services;

namespace OWP.UserManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserManagementController : ControllerBase
{
    private readonly INapSession currentSession;
    private readonly ExportService exportService;
    private readonly ModelService modelService;
    private readonly RefResourceService refResourceService;
    private readonly Repository repository;

    private readonly UsersService usersService;

    public UserManagementController(
        ExportService exportService,
        ModelService modelService,
        Repository repository,
        UsersService usersService,
        INapSession currentSession, RefResourceService refResourceService)
    {
        this.exportService = exportService;
        this.modelService = modelService;
        this.repository = repository;
        this.currentSession = currentSession;
        this.refResourceService = refResourceService;
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

        return new FilterViewModel
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
    }

    [HttpGet("query-filter-options")]
    [NapAuth(FunctionKey = FunctionKeys.UserManagement, Policy = Constant.Policy.Read)]
    public async Task<FilterViewModel> QueryFilterOptions()
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


    [HttpGet("query-current-section")]
    [NapAuth(FunctionKey = FunctionKeys.UserManagement, Policy = Constant.Policy.Read)]
    public async Task<ReferenceResource> QuerySectionName()
    {
        var userId = currentSession.UserId;
        var userSectionId = await repository.GetSectionIdByUserId(userId);
        var allResource = await refResourceService.GetAllReferenceResource();
        var currentSection = allResource.FirstOrDefault(x =>
            x.Name == ReferenceResourceNames.Section && x.ResourceId == userSectionId);
        return currentSection ?? new()
        {
            ResourceId = 0,
            Value = string.Empty,
            Name = string.Empty
        };
    }

    [HttpGet("query-level-options")]
    [NapAuth(FunctionKey = FunctionKeys.UserManagement, Policy = Constant.Policy.Read)]
    public async Task<object> QueryLevelOption()
    {
        var levelDefault = await repository.GetLevelBySectionId(null);
        var levelLiveTrading = await repository.GetLevelBySectionId(3);
        var levelLiveAssistant = await repository.GetLevelBySectionId(30);
        return new
        {
            LevelOfLiveAssistant = levelLiveAssistant,
            LevelOfLiveTrading = levelLiveTrading,
            LevelOfOtherSection = levelDefault
        };
    }

    [HttpGet("export-excel")]
    [NapAuth(FunctionKey = FunctionKeys.UserManagement, Policy = Constant.Policy.Read)]
    public async Task<IActionResult> ExportToExcel([FromQuery] QueryModel model)
    {
        var users = await repository.GetUserPagingAsync(model);
        ExportHelper.BuildCorrectDataToExport(users);
        var exportDate = DateTime.Now;
        byte[] bytes;
        using (var stream = new MemoryStream())
        {
            exportService.ExportToExcel(stream, users, exportDate);
            bytes = stream.ToArray();
        }

        var fileName = $"UserManagement-{exportDate:yyyyMMdd}.xlsx";
        return File(bytes, "application/xlsx", fileName);
    }

    [NapAuth(FunctionKey = FunctionKeys.UserManagement, Policy = Constant.Policy.Read)]
    [HttpPost]
    [Route("query-user")]
    [ServiceFilter(typeof(GetUserValidator))]
    public async Task<IList<User>> GetUsers(QueryModel model)
    => await usersService.GetUserPagingAsync(model);

    [HttpPost]
    [Route("update-user-info")]
    public async Task<UpdateUserInfo> UpdateUserInfo(UpdateUserInfo info)
    {
        await usersService.UpdateUserInfo(info);
        return info;
    }

}