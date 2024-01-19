namespace OWP.Event.LiveGameAssignment.Download.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OWP.Models;
using OWP.Services;
using Service.Implement;

[Route("api/event-picking-old")]
//[ApiController]
//[EnableCors]
[AllowAnonymous]
public class EventPickingOldController : ControllerBase
{
    private readonly LeagueSettingService leagueSettingService;
    private readonly EventPickingService eventPickingService;
    private readonly AccessCounter accessCounter;
    private readonly AppSettings settings;

    public EventPickingOldController(
        LeagueSettingService leagueSettingService,
        EventPickingService eventPickingService,
        AccessCounter accessCounter,
        IOptions<AppSettings> options)
    {
        this.leagueSettingService = leagueSettingService;
        this.eventPickingService = eventPickingService;
        this.accessCounter = accessCounter;
        settings = options.Value;
    }

    //[NapAuth(FunctionKey = FunctionKeys.EventPicking, Policy = Constant.Policy.Read)]
    //public async Task<ActionResult> Index()
    //{
    //    var model = new EventPickingViewModel
    //    {
    //        Date = DateTime.Now
    //    };
    //    var functionName = ConfigurationManager.AppSettings["EventPicking"];
    //    await accessCounter.UpdateCounter(functionName);

    //    return View("Index", model);
    //}

    //[HttpGet]
    //[NapAuth(FunctionKey = FunctionKeys.EventPicking, Policy = Constant.Policy.Read)]
    //public async Task<FileResult> SyncedEventPickingExcel(DateTime date)
    //{
    //    var templatePath = Path.Combine("App_Data", "Templates", "EventPicking.xlsx");
    //    var functionName = ConfigurationManager.AppSettings["LiveGameAssignment_Export_Event_Picking"];
    //    await accessCounter.UpdateCounter(functionName);
    //    return await DownloadExcel(templatePath, date);
    //}

    //[HttpGet]
    //[NapAuth(FunctionKey = FunctionKeys.EventPicking, Policy = Constant.Policy.Read)]
    //public async Task<FileResult> ExportEventPicking(DateTime date)
    //{
    //    var templatePath = Path.Combine("App_Data", "Templates", "EventPickingNewFormat.xlsx");
    //    return await eventPickingService.ExportEventPicking(date, templatePath);
    //}

    //[HttpPost]
    //[NapAuth(FunctionKey = FunctionKeys.EventPicking, Policy = Constant.Policy.Read)]
    //public async Task<ActionResult> EventPickingExcel(EventPickingViewModel model)
    //{
    //    var functionName = ConfigurationManager.AppSettings["EventPicking_Export_Excel"];
    //    await accessCounter.UpdateCounter(functionName);
    //    var date = model.Date;
    //    var templatePath = Path.Combine("App_Data", "Templates", "EventPicking.xlsx");

    //    return await DownloadExcel(templatePath, date);
    //}

    //private async Task<FileStreamResult> DownloadExcel(string templatePath, DateTime date)
    //{
    //    var nextDate = date.AddDays(1);
    //    var mainMatches = GetMainMatch(date, nextDate);
    //    var pullingChildMatch = new PullingChildMatch();
    //    var childMatches = pullingChildMatch.Query(settings.InfoSiteAPI, date, nextDate);
    //    var matches = CombineSubMatch(mainMatches, childMatches);
    //    var syncedMatches = await leagueSettingService.ApplyMatchMaxbet(matches);
    //    using (var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
    //    {
    //        var excelPackage = new ExcelPackage(stream);
    //        var templateSheet = excelPackage.Workbook.Worksheets.First();
    //        templateSheet.ApplyVertical(syncedMatches, 2);

    //        var fileStream = new MemoryStream();
    //        excelPackage.SaveAs(fileStream);
    //        fileStream.Position = 0;
    //        const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    //        var fsr = new FileStreamResult(fileStream, contentType);
    //        fsr.FileDownloadName = $"Event Picking-{date:yyyy-MM-dd}.xlsx";
    //        return fsr;
    //    }
    //}

    //private List<MatchExporting> GetMainMatch(DateTime fromDate, DateTime toDate)
    //{
    //    var timeFrom = fromDate.AddHours(12);
    //    var timeTo = toDate.AddHours(12);
    //    var pullingMainMatch = new PullingMainMatch();
    //    var mainRawMatches = new List<CommonMatchEx>();
    //    var date = fromDate;
    //    while (date <= toDate)
    //    {
    //        mainRawMatches.AddRange(pullingMainMatch.Query(settings.InfoSiteAPI, date, date, DateTime.Today.AddYears(-10)));
    //        date = date.AddDays(1);
    //    }

    //    var mainMatches = mainRawMatches
    //                        .Select(e => new MatchExporting(e))
    //                        .Where(e => e.GameDate >= timeFrom && e.GameDate < timeTo)
    //                        .ToList();
    //    return mainMatches;
    //}

    //private static List<MatchExporting> CombineSubMatch(IList<MatchExporting> mainMatches, IList<ChildMatch> childMatches)
    //{
    //    var result = new List<MatchExporting>();
    //    foreach (var mainMatch in mainMatches)
    //    {
    //        result.Add(mainMatch);
    //        var childMatchesOfThisMainMatch = childMatches.Where(m => m.match_id == mainMatch.InfoSiteMatchId);
    //        foreach (var childMatch in childMatchesOfThisMainMatch)
    //        {
    //            var match = new MatchExporting(childMatch);
    //            result.Add(match);
    //        }
    //    }
    //    return result;
    //}
}