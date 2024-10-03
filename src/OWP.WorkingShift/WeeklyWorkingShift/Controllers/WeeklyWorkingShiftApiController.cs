using Fanex.NAP.Client;
using Newtonsoft.Json;

namespace OWP.WorkingShift.WeeklyWorkingShift.Controllers;

using Models;
using Services;
using Repository = Repositories.Repository;

[Route("api/weekly-working-shift")]
[ApiController]
[Authorize]

public class WeeklyWorkingShiftApiController : ControllerBase
{
    private readonly Repository weeklyWorkingShiftRepository;
    private readonly Common.Services.ModelService modelService;
    private readonly ImportService importService;
    private readonly ExportService exportService;
    private readonly INapSession currentSession;
    private readonly ViewLogService viewLogService;

    public WeeklyWorkingShiftApiController(
        Repository weeklyWorkingShiftRepository,
        Common.Services.ModelService modelService,
        ExportService exportService,
        ImportService importService,
        ViewLogService viewLogService,
        INapSession currentSession
    )
    {
        this.weeklyWorkingShiftRepository = weeklyWorkingShiftRepository;
        this.modelService = modelService;
        this.exportService = exportService;
        this.viewLogService = viewLogService;
        this.importService = importService;
        this.currentSession = currentSession;
    }

    [Route("search")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement, Policy = Constant.Policy.Read)]
    public async Task<JsonResult> SearchWorkingShiftWeekly(QueryModel model)
    {
        model.IsFrame = false;
        model.Permission = modelService.BuildPermissionViewModel();
        model.OrderUserId = modelService.GetOrderUserId(model.Permission);
        if (!model.Permission.WorkingShiftManagementOwnSection.IsRead && !model.Permission.WorkingShiftManagementAllSections.IsRead)
        {
            model.User = currentSession.UserName;
        }

        var result = await weeklyWorkingShiftRepository.SearchWorkingShiftWeekly(model);
        return new JsonResult(result, new JsonSerializerSettings());
    }

    [Route("export")]
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ExportWorkingShift([FromQuery] ExportModel model)
    {
        model.Permission = modelService.BuildPermissionViewModel();
        model.OrderUserId = modelService.GetOrderUserId(model.Permission);
        using (var stream = new MemoryStream())
        {
            await exportService.Export(stream, model);

            return File(stream.ToArray(), "application/octet-stream", model.GetExportFileName());
        }
    }

    [Route("validate")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> ValidateWorkingShift()
    {
        if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
        {
            return new UnsupportedMediaTypeResult();
        }

        if (Request.Form.Files.Count != 1)
        {
            return BadRequest();
        }

        await using var ms = new MemoryStream();
        await Request.Form.Files[0].CopyToAsync(ms);

        var result = await importService.Validate(ms);
        return new JsonResult(result, new JsonSerializerSettings());
    }

    [Route("upload")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> ImportWorkingShift(IList<WorkingShiftImportEntity> shifts)
    {
        await importService.Import(shifts);
        return Ok();
    }

    [Route("viewlog")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.WorkingShiftManagement, Policy = Constant.Policy.Read)]
    public async Task<string> ViewLog(ViewLogModel model)
    {
        return await viewLogService.ViewLog(model);
    }
}