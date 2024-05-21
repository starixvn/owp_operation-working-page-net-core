namespace OWP.Event.LiveGameAssignment.EventPicking;

using Epplus.Mapper.Extensions;
using Fanex.Logging;
using Fanex.NAP.Client.Attributes;
using Fanex.NAP.Client.Helpers.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using Olympus.InfoSiteClient;
using Olympus.InfoSiteClient.Models;
using OWP.Constants;
using OWP.Event.LiveGameAssignment.EventPicking.Model;
using OWP.Event.LiveGameAssignment.EventPicking.Model.ViewModels;
using OWP.Event.LiveGameAssignment.EventPicking.Service;
using OWP.Models;
using OWP.Services;
using System.Configuration;

[Route("api/event-assignment/event-picking-old")]
[ApiController]
[EnableCors]
[Authorize]
public class EventPickingOldController : ControllerBase
{
    private readonly LeagueSettingService leagueSettingService;
    private readonly AccessCounter accessCounter;
    private readonly AppSettings settings;

    private readonly ILogger logger;

    public EventPickingOldController(
        LeagueSettingService leagueSettingService,
        AccessCounter accessCounter,
        IOptions<AppSettings> options,
        ILogger logger)
    {
        this.leagueSettingService = leagueSettingService;
        this.accessCounter = accessCounter;
        settings = options.Value;
        this.logger = logger;
    }

    [Route("sync-event-picking")]
    [HttpGet]
    [NapAuth(FunctionKey = FunctionKeys.EventPicking, Policy = Constant.Policy.Read)]
    public async Task<FileResult> SyncedEventPickingExcel(DateTime date)
    {
        var templatePath = Path.Combine("App_Data", "Templates", "EventPicking.xlsx");
        var functionName = ConfigurationManager.AppSettings["LiveGameAssignment_Export_Event_Picking"];
        await accessCounter.UpdateCounter(functionName);
        return await DownloadExcel(templatePath, date);
    }

    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.EventPicking, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> EventPickingExcel(EventPickingViewModel model)
    {
        var functionName = ConfigurationManager.AppSettings["EventPicking_Export_Excel"];
        await accessCounter.UpdateCounter(functionName);
        var date = model.Date;
        var templatePath = Path.Combine("App_Data", "Templates", "EventPicking.xlsx");

        return await DownloadExcel(templatePath, date);
    }

    private async Task<FileStreamResult> DownloadExcel(string templatePath, DateTime date)
    {
        await logger.InfoAsync("InfoSiteAPI=" + settings.InfoSiteAPI + " date=" + date);

        var nextDate = date.AddDays(1);
        var mainMatches = await GetMainMatch(date, nextDate);
        var pullingChildMatch = new PullingChildMatch();
        var childMatches = pullingChildMatch.Query(settings.InfoSiteAPI, date, nextDate);
        var matches = CombineSubMatch(mainMatches, childMatches);
        var syncedMatches = await leagueSettingService.ApplyMatchMaxbet(matches);
        using (var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
        {
            var excelPackage = new ExcelPackage(stream);
            var templateSheet = excelPackage.Workbook.Worksheets.First();
            templateSheet.ApplyVertical(syncedMatches, 2);

            var fileStream = new MemoryStream();
            excelPackage.SaveAs(fileStream);
            fileStream.Position = 0;
            const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fsr = new FileStreamResult(fileStream, contentType);
            fsr.FileDownloadName = $"Event Picking-{date:yyyy-MM-dd}.xlsx";
            return fsr;
        }
    }

    private async Task<List<MatchExporting>> GetMainMatch(DateTime fromDate, DateTime toDate)
    {
        var timeFrom = fromDate.AddHours(12);
        var timeTo = toDate.AddHours(12);
        var pullingMainMatch = new PullingMainMatch();
        var mainRawMatches = new List<CommonMatchEx>();
        var date = fromDate;
        while (date <= toDate)
        {
            mainRawMatches.AddRange(pullingMainMatch.Query(settings.InfoSiteAPI, date, date, DateTime.Today.AddYears(-10))); //NOSONAR
            date = date.AddDays(1);
        }

        var mainMatches = mainRawMatches
                            .Select(e => new MatchExporting(e))
                            .Where(e => e.GameDate >= timeFrom && e.GameDate < timeTo)
                            .ToList();
        return mainMatches;
    }

    private static List<MatchExporting> CombineSubMatch(IList<MatchExporting> mainMatches, IList<ChildMatch> childMatches)
    {
        var result = new List<MatchExporting>();
        foreach (var mainMatch in mainMatches)
        {
            result.Add(mainMatch);
            var childMatchesOfThisMainMatch = childMatches.Where(m => m.match_id == mainMatch.InfoSiteMatchId);
            foreach (var childMatch in childMatchesOfThisMainMatch)
            {
                var match = new MatchExporting(childMatch);
                result.Add(match);
            }
        }
        return result;
    }
}