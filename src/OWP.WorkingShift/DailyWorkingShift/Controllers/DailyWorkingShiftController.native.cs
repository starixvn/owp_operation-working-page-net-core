using Fanex.NAP.Client;
using OWP.WorkingShift.DailyWorkingShift.ViewModels;

namespace OWP.WorkingShift.DailyWorkingShift.Controllers;



public class DailyWorkingShiftController : Controller
{
    private readonly RefResourceService resourceRepository;
    private readonly ModelService modelService;
    private readonly INapSession currentSession;
    private readonly UserRepository userRepository;
    private readonly AccessCounter accessCounter;

    public DailyWorkingShiftController(RefResourceService resourceRepository,
        ModelService modelService,
        INapSession currentSession,
        UserRepository userRepository, AccessCounter accessCounter)
    {
        this.resourceRepository = resourceRepository;
        this.modelService = modelService;
        this.currentSession = currentSession;
        this.userRepository = userRepository;
        this.accessCounter = accessCounter;
    }

    [NapAuth(FunctionKey = FunctionKeys.DailyWorkingShift, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> Index()
    {
        var permission = modelService.BuildPermissionViewModel();
        var sections = (await resourceRepository.GetResourceByNameWithNone(ReferenceResourceNames.Section))
            .Where(x => !HiddenSectionConst.HiddenSections.Exists(i => i == x.ResourceId)).ToList();
        var departments = await resourceRepository.GetResourceByNameWithNone(ReferenceResourceNames.Department);
        var user = await userRepository.GetUserInfoById(currentSession.UserId);
        if (!permission.DailyWorkingShiftAllSections.IsRead && permission.DailyWorkingShiftOwnSection.IsRead)
        {
            sections = sections.Where(x => x.ResourceId == user.SectionId).ToList();
        }

        var model = new DailyViewModel
        {
            Permission = permission,
            Date = DateTime.Today,
            Sections = sections,
            Departments = departments,
            UserLocations = resourceRepository.GetAllLocations(),
            Username = currentSession.UserName
        };
        return View(model);
    }
}