
using Fanex.NAP.Client;
using OWP.WorkingShift.Common.Repositories;
namespace OWP.WorkingShift.WeeklyWorkingShift.Controllers;

using Constants;
using Models;
using System;
using System.Linq;
using System.Threading.Tasks;

public class WeeklyWorkingShiftController : Controller
{
    private readonly RefResourceService resourceRepository;
    private readonly Common.Services.ModelService modelService;
    private readonly INapSession currentSession;
    private readonly WorkingShiftTimezoneSettingRepository timezoneSettingRepository;
    private readonly UserRepository userRepository;
    private readonly AccessCounter accessCounter;

    public WeeklyWorkingShiftController(
        RefResourceService resourceRepository,
        Common.Services.ModelService modelService,
        INapSession currentSession,
        WorkingShiftTimezoneSettingRepository timezoneSettingRepository,
        UserRepository userRepository, AccessCounter accessCounter)
    {
        this.modelService = modelService;
        this.resourceRepository = resourceRepository;
        this.currentSession = currentSession;
        this.timezoneSettingRepository = timezoneSettingRepository;
        this.userRepository = userRepository;
        this.accessCounter = accessCounter;
    }

    [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> Index()
    {
        var currentUserId = currentSession.UserId;
        var permission = modelService.BuildPermissionViewModel();
        var sections = (await resourceRepository.GetResourceByNameWithNone(ReferenceResourceNames.Section))
            .Where(x => !HiddenSectionConst.HiddenSections.Exists(i => i == x.ResourceId)).ToList();
        var departments = await resourceRepository.GetResourceByNameWithNone(ReferenceResourceNames.Department);
        var user = await userRepository.GetUserInfoById(currentSession.UserId);
        var currentSectionId = user.SectionId;
        if (!permission.WorkingShiftManagementAllSections.IsRead)
        {
            sections = sections.Where(x => x.ResourceId == user.SectionId).ToList();
        }

        var model = new FilterViewModel
        {
            Permission = permission,
            FromDate = DateTimeOffset.Now.Date,
            ToDate = DateTimeOffset.Now.Date,
            Sections = sections,
            Departments = departments,
            UserCategories = (await resourceRepository.GetRefResourcesByName(ReferenceResourceNames.UserCategory)).ToList(),
            UserLocations = resourceRepository.GetAllLocations(),
            TimezoneSettings = await timezoneSettingRepository.GetTimezoneSettings(),
            CurrentUserId = currentUserId,
            CurrentSectionId = currentSectionId ?? 0,
            IsFrame = false
        };

        await accessCounter.UpdateCounter(AccessCounterConst.WorkingShiftManagement);
        return View(model);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult> IFrame(float? gmt = 7)
    {
        var sections = (await resourceRepository.GetResourceByNameWithNone(ReferenceResourceNames.Section))
            .Where(x => !HiddenSectionConst.HiddenSections.Exists(i => i == x.ResourceId)).ToList();
        var departments = await resourceRepository.GetResourceByNameWithNone(ReferenceResourceNames.Department);
        const int defaultMinus = 60;
        const float defaultGTM7 = 7;
        const float defaultGMT8 = 8;

        var defaultGMT = gmt ?? defaultGTM7;

        var convertOffset = (defaultGMT - defaultGMT8) * defaultMinus;

        var getTimezoneSettings = await timezoneSettingRepository.GetTimezoneSettings();
        if (!getTimezoneSettings.Select(x => x.Offset).Contains((int)convertOffset))
        {
            convertOffset = (defaultGTM7 - defaultGMT8) * defaultMinus;
        }

        foreach (var item in getTimezoneSettings)
        {
            item.IsSelected = item.Offset == convertOffset;
        }

        var selectedTimezoneSetting = getTimezoneSettings.FirstOrDefault(x => x.IsSelected);
        var model = new FilterViewModel
        {
            FromDate = DateTimeOffset.Now.Date,
            ToDate = DateTimeOffset.Now.Date,
            Sections = sections,
            Departments = departments,
            UserCategories = (await resourceRepository.GetRefResourcesByName(ReferenceResourceNames.UserCategory)).ToList(),
            UserLocations = resourceRepository.GetAllLocations(),
            TimezoneSettings = getTimezoneSettings,
            IsFrame = true,
            Permission = new WorkingShiftPermission()
        };

        ViewBag.selectedTimezoneSetting = selectedTimezoneSetting ?? new TimezoneSettingModel { Name = "GMT+7", Offset = -defaultMinus };
        return View(model);
    }
}