namespace OWP.OtherSport.Download.Services;

using Epplus.Mapper.Extensions;
using OfficeOpenXml;
using OWP.Extensions;
using OWP.OtherSport.Download.Constants;
using OWP.OtherSport.Download.Models;
using OWP.OtherSport.Submit.Models;
using OWP.Services;

public interface ExportExcelService
{
    Task<ExcelPackage> CreateExcelPackage(DownloadAssignmentRequest request);
}

public class ExportExcelServiceImp : ExportExcelService, IDisposable
{
    private readonly ConfigurationService configurationService;

    private readonly BuildExcelModelService buildExcelModelService;

    private ExcelPackage excelPackage;

    private ExcelWorksheet reportSheet;

    public ExportExcelServiceImp(
        ConfigurationService configurationService,
        BuildExcelModelService buildExcelModelService)
    {
        this.configurationService = configurationService;
        this.buildExcelModelService = buildExcelModelService;
    }

    public async Task<ExcelPackage> CreateExcelPackage(DownloadAssignmentRequest request)
    {
        var templatePath = @$"App_Data{Path.DirectorySeparatorChar}Templates{Path.DirectorySeparatorChar}ExportOSAssignmentTemplate.xlsx";
        var absoluteTemplatePath = $"{configurationService.CurrentDirectory}{Path.DirectorySeparatorChar}{templatePath}";

        var model = await buildExcelModelService.BuildExcelModel(request);

        return CreateExcelPackage(absoluteTemplatePath, model);
    }

    public ExcelPackage CreateExcelPackage(string templatePath, ExcelModel model)
    {
        LoadTemplate(templatePath);
        BuildLAReportBody(model);
        ClearTemplate();
        return excelPackage;
    }

    public void LoadTemplate(string templatePath)
    {
        using var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read);

        excelPackage = new ExcelPackage(stream);
        reportSheet = excelPackage.Workbook.Worksheets.Copy(ExcelConst.TemplateName, ExcelConst.SheetName);
    }

    public void BuildLAReportBody(ExcelModel model)
    {
        reportSheet.ApplyModel(model.HeaderInfo);
        BuildAssignments(model.ExcelAssignmentModels);
    }

    public void ClearTemplate()
    {
        excelPackage.Workbook.Worksheets.Delete(ExcelConst.TemplateName);
    }

    private void BuildAssignments(ExcelAssignmentModel model)
    {
        for (int i = 0, row = ExcelConst.StartRowIndex; i < model.Matches.Count; i++, row++)
        {
            var match = model.Matches[i];
            var assignments = model.MatchAssignments.GetValue(match.OSMatchId, new List<ExcelMatchAssignment>());
            var maxbetSpreads = model.MaxBetSpread.GetValue(match.OSMatchId, new List<ExcelMatchMaxBetSpread>());

            BuildMatchInfo(row, match);
            BuildTrader(row, assignments);
            BuildMatchSettings(row, maxbetSpreads);
        }
    }

    private void BuildMatchInfo(int row, MatchInfo assignment)
    {
        reportSheet.Cells[row, MatchInfoConst.MatchNoColIndex].Value = assignment.MatchNo;
        reportSheet.Cells[row, MatchInfoConst.AdminsiteIndex].Value = assignment.AdminsiteMatchId;
        reportSheet.Cells[row, MatchInfoConst.MatchStatusColIndex].Value = assignment.OfferLive;
        reportSheet.Cells[row, MatchInfoConst.SportColIndex].Value = assignment.Sport;
        reportSheet.Cells[row, MatchInfoConst.DateTimeColIndex].Value = assignment.DateTime;
        reportSheet.Cells[row, MatchInfoConst.LeagueColIndex].Value = assignment.League;
        reportSheet.Cells[row, MatchInfoConst.HomeColIndex].Value =
            GetHomeName(assignment.Home, assignment.IsNeutral);
        reportSheet.Cells[row, MatchInfoConst.AwayColIndex].Value = assignment.Away;
        reportSheet.Cells[row, MatchInfoConst.CategoryColIndex].Value = assignment.Category;
        reportSheet.Cells[row, MatchInfoConst.NoHDPColIndex].Value = assignment.NoHDP;
        reportSheet.Cells[row, MatchInfoConst.MatchOfferColIndex].Value = assignment.MatchOffer;
        reportSheet.Cells[row, MatchInfoConst.AutoLinkColIndex].Value = assignment.AutoLink;
        reportSheet.Cells[row, MatchInfoConst.ProviderColIndex].Value = assignment.Provider;
        reportSheet.Cells[row, MatchInfoConst.OtherProviderColIndex].Value = assignment.OtherProvider;
        reportSheet.Cells[row, MatchInfoConst.ServiceColIndex].Value = assignment.Service;
        reportSheet.Cells[row, MatchInfoConst.IPTVColIndex].Value = assignment.IPTV;
        reportSheet.Cells[row, MatchInfoConst.WBColIndex].Value = assignment.WB;
        reportSheet.Cells[row, MatchInfoConst.ReporterColIndex].Value = assignment.Reporter;
        reportSheet.Cells[row, MatchInfoConst.RemarkColIndex].Value = assignment.Remark;
    }

    private void BuildTrader(int row, List<ExcelMatchAssignment> assignments)
    {
        foreach (var assignment in assignments)
        {
            reportSheet.Cells[row, assignment.ColumnIndex].Value = assignment.Value;
        }
    }

    private string GetHomeName(string homeName, bool isNeutral)
        => isNeutral ? homeName + " (N)" : homeName;

    private void BuildMatchSettings(int row, List<ExcelMatchMaxBetSpread> matchSettings)
    {
        foreach (var matchSetting in matchSettings)
        {
            reportSheet.Cells[row, matchSetting.ColumnIndexMaxbet].Value = matchSetting.ValueMaxbet;
            reportSheet.Cells[row, matchSetting.ColumnIndexSpread].Value = matchSetting.ValueSpread;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool isDisposing)
    {
        excelPackage?.Dispose();
    }
}