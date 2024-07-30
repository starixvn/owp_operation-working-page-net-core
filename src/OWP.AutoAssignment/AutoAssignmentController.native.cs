using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OWP.AutoAssignment.Constant;
using OWP.AutoAssignment.Models.InternalApi;
using OWP.AutoAssignment.Models.PublicApi;
using OWP.AutoAssignment.Services.InternalApi;

namespace OWP.AutoAssignment;

[Route("api/auto-assignment")]
[ApiController]
[Authorize]
public class AutoAssignmentController : ControllerBase
{
    private readonly CreateRequestService createRequestService;

    private readonly ExcelPackageCreatorService excelPackageService;

    private readonly ExportService exportService;

    public AutoAssignmentController(
        CreateRequestService createRequestService,
        ExcelPackageCreatorService excelPackageService,
        ExportService exportService)
    {
        this.createRequestService = createRequestService;
        this.excelPackageService = excelPackageService;
        this.exportService = exportService;
    }

    [Route("remove-request")]
    [HttpPost]
    public async Task RemoveRequest(CheckRequestModel model)
        => await createRequestService.RemoveRequest(model.RequestId);

    [Route("get-request")]
    [HttpGet]
    public async Task<IEnumerable<SchedulingRequest>> GetRequests()
        => await createRequestService.GetRequests();

    [Route("check-request")]
    [HttpPost]
    public async Task<int> CheckRequest(CheckRequestModel model)
        => await createRequestService.GetRequestStatus(model.RequestId);

    [Route("create-request")]
    [HttpPost]
    public async Task<IActionResult> CreateRequest(CreateRequestModel model)
    {
        var existsMatchValid = await createRequestService.ExistMatchValid(model.EventDate);
        var existsTraderValid = await createRequestService.ExistsSuspenderValid(model.EventDate);
        var requestIdIsWaiting = await createRequestService.GetRequestIsWaiting(model.EventDate);
        if (!string.IsNullOrWhiteSpace(requestIdIsWaiting))
        {
            var message = string.Format(AutoConstant.ExistsRequestIdIsWaiting, requestIdIsWaiting);
            return BadRequest(message);
        }
        if (!existsMatchValid)
        {
            return BadRequest(AutoConstant.NoFindAnyMatchValidOnSelectedDate);
        }
        if (!existsTraderValid)
        {
            return BadRequest(AutoConstant.NoFindAnyTraderValidOnSelectedDate);
        }
        await createRequestService.CreateRequest(model);
        return Ok();
    }

    [Route("export-excel")]
    [HttpGet]
    public async Task<IActionResult> ExportExcel(DateTime eventDate)
    {
        var exportModel = await exportService.BuildExportModelByDate(eventDate);
        var exportExcelModel = new ExportExcelModel
        {
            Date = eventDate.ToString("MMM dd, yyyy"),
            exportMatchModels = exportModel
        };

        var absoluteTemplatePath = @$"App_Data{Path.DirectorySeparatorChar}Templates{Path.DirectorySeparatorChar}AutoAssignment.xlsx";

        using var excelPackage = excelPackageService.CreateExcelPackage(absoluteTemplatePath, exportExcelModel);
        var fileContent = await excelPackage.GetAsByteArrayAsync();

        return BuildExcelPackageResponse(fileContent, eventDate);
    }

    private IActionResult BuildExcelPackageResponse(byte[] fileContent, DateTime eventDate)
    {
        var fileName = $"Auto_Assignment_{eventDate:yyyyMMdd}.xlsx";
        return File(fileContent, "application/xlsx", fileName);
    }
}