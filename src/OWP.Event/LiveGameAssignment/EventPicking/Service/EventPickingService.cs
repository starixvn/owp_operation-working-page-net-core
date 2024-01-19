using Epplus.Mapper.Extensions;
using Fanex.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Olympus.InfoSiteClient;
using Olympus.InfoSiteClient.Models;
using OWP.Event.LiveGameAssignment.Download.Builder;
using OWP.Event.LiveGameAssignment.Download.Misc;
using OWP.Event.LiveGameAssignment.Download.Service.Implement;
using OWP.Event.LiveGameAssignment.EventPicking.Criteria;
using OWP.Event.LiveGameAssignment.EventPicking.Model;
using OWP.Models;
using System.Drawing;

namespace OWP.Event.LiveGameAssignment.EventPicking.Service;

public interface EventPickingService
{
    Task<IList<EventInfo>> GetEventInfo(string infoSiteIds);

    Task<FileResult> ExportEventPicking(DateTime date, string templatePath);

    void MapOwpInfoToEventPicking(List<MatchExportingNewFormat> eventsExporting, List<EventInfo> eventSelfInfo);
}

public class EventPickingServiceImp : EventPickingService
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly LeagueSettingNewFormatService leagueSettingNewFormatService;
    private readonly ExcelService excelService;
    private readonly BuildEventInfoService buildEventInfoService;
    private readonly OrderMatchService orderMatchService;
    private readonly AppSettings settings;

    public EventPickingServiceImp(
        IDynamicRepository dynamicRepository,
        BuildEventInfoService buildEventInfoService,
        LeagueSettingNewFormatService leagueSettingNewFormatService,
        ExcelService excelService,
        OrderMatchService orderMatchService,
        IOptions<AppSettings> options)
    {
        this.dynamicRepository = dynamicRepository;
        this.buildEventInfoService = buildEventInfoService;
        this.leagueSettingNewFormatService = leagueSettingNewFormatService;
        this.excelService = excelService;
        this.orderMatchService = orderMatchService;
        settings = options.Value;
    }

    public async Task<IList<EventInfo>> GetEventInfo(string infoSiteIds)
    {
        var criteria = new GetEventInfoCriteria
        {
            InfoSiteIds = infoSiteIds
        };

        var result = await dynamicRepository.FetchMultipleAsync<EventInfo, MatchAssignment, MatchSetting>(criteria);
        var matches = result.Item1.ToList();
        var matchAssignments = result.Item2.ToList();
        var matchSettings = result.Item3.ToList();

        return buildEventInfoService.BuildEventInfo(matches, matchAssignments, matchSettings);
    }

    public void MapOwpInfoToEventPicking(
        List<MatchExportingNewFormat> eventsExporting
        , List<EventInfo> eventSelfInfo)
    {
        eventsExporting.ForEach(evt =>
        {
            MapEventInfo(evt, eventSelfInfo);
        });
    }

    public static void MapEventInfo(
        MatchExportingNewFormat eventsExporting
        , IList<EventInfo> eventSelfInfo)
    {
        var eventInfo = eventSelfInfo?.FirstOrDefault(e => e.InfositeId == eventsExporting.InfoSiteID);
        if (eventInfo != null)
        {
            eventsExporting.SetMatchInfo(eventInfo);
        }
    }

    public async Task<FileResult> ExportEventPicking(DateTime date, string templatePath)
    {
        var nextDate = date.AddDays(1);
        var mainMatches = GetMainEvent(date, nextDate);
        var pullingChildMatch = new PullingChildMatch();
        var childMatches = pullingChildMatch.Query(settings.InfoSiteAPI, date, nextDate);
        var matches = CombineSubEvent(mainMatches, childMatches);
        var infoSiteIds = matches.Select(m => m.InfoSiteID);
        var eventInfo = await GetEventInfo(string.Join(",", infoSiteIds));
        MapOwpInfoToEventPicking(matches, eventInfo.ToList());
        await leagueSettingNewFormatService.ApplyMatchMaxbet(matches);
        await leagueSettingNewFormatService.ApplyIsBlackList(matches);
        excelService.BuildRowColor(matches);
        var matchOrderable = orderMatchService.Order(matches);
        using (var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
        {
            var excelPackage = new ExcelPackage(stream);
            var templateSheet = excelPackage.Workbook.Worksheets.First();
            templateSheet.ApplyVertical(matchOrderable, 2);
            FillRowColor(matchOrderable, templateSheet);

            var fileStream = new MemoryStream();
            excelPackage.SaveAs(fileStream);
            fileStream.Position = 0;
            const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fsr = new FileStreamResult(fileStream, contentType);
            fsr.FileDownloadName = $"Event Picking-{date.ToString("yyyy-MM-dd")}.xlsx";
            return fsr;
        }
    }

    private static void FillRowColor(IList<MatchExportingNewFormat> matches, ExcelWorksheet templateSheet)
    {
        for (int i = 0, row = 2; i < matches.Count; i++, row++)
        {
            var match = matches[i];
            for (int column = 1; column <= 90; column++)
            {
                if (match.RowColor != null && match.RowColor != Constant.ColorTransparent)
                {
                    var colorTraderFromHex = ColorTranslator.FromHtml(match.RowColor);
                    templateSheet.Cells[row, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    templateSheet.Cells[row, column].Style.Fill.BackgroundColor.SetColor(colorTraderFromHex);
                }
            }
        }
    }

    private IList<MatchExportingNewFormat> GetMainEvent(DateTime fromDate, DateTime toDate)
    {
        var timeFrom = fromDate.AddHours(12);
        var timeTo = toDate.AddHours(12);
        var pullingMainMatch = new PullingMainMatch();
        var mainRawMatches = new List<CommonMatchEx>();
        var date = fromDate;
        while (date <= toDate)
        {
            mainRawMatches.AddRange(pullingMainMatch.Query(settings.InfoSiteAPI, date, date, DateTime.Today.AddYears(-10)));
            date = date.AddDays(1);
        }

        var mainMatches = new List<MatchExportingNewFormat>();
        foreach (var mainRawMatch in mainRawMatches)
        {
            var mainMatch = new MatchExportingNewFormat();
            mainMatch.MapInfoSiteToMainMatch(mainRawMatch);
            if (mainMatch.GameDate >= timeFrom && mainMatch.GameDate < timeTo)
            {
                mainMatches.Add(mainMatch);
            }
        }
        return mainMatches;
    }

    private static List<MatchExportingNewFormat> CombineSubEvent(IList<MatchExportingNewFormat> mainMatches, IList<ChildMatch> childMatchesRaw)
    {
        var result = new List<MatchExportingNewFormat>();
        var childMatches = SpecialMatchBuilder.ReduceSpecialMatch(childMatchesRaw);
        foreach (var mainMatch in mainMatches)
        {
            result.Add(mainMatch);
            var childMatchesOfThisMainMatch = childMatches.Where(m => m.match_id == mainMatch.InfoSiteID);
            foreach (var childMatch in childMatchesOfThisMainMatch)
            {
                var match = new MatchExportingNewFormat();
                match.MapInfoSiteToChildMatch(childMatch);
                result.Add(match);
            }
        }
        return result
            .Where(x => !IsETPKMatch(x.HomeName))
            .ToList();
    }

    private static bool IsETPKMatch(string homeName)
        => homeName.IsContains("(ET)") || homeName.IsContains("(PEN)");
}
