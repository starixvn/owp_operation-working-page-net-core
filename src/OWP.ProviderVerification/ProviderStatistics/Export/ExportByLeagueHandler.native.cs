using Epplus.Mapper.Extensions;
using OfficeOpenXml.Style;
using OWP.ProviderVerification.ProviderStatistics.Criteria;
using OWP.ProviderVerification.ProviderStatistics.Models;
using System.Drawing;
using OWP.Constants;

namespace OWP.ProviderVerification.ProviderStatistics.Export;

public record ExportByLeagueRequest(DateTime FromDate, DateTime ToDate, string Template) : Query<ExcelPackage>;

public class ExportByLeagueHandler(ConfigurationService configuration, IDynamicRepository repository) : QueryHandler<ExportByLeagueRequest, ExcelPackage>
{
    private ExcelPackage ExcelPackage { get; set; }
    private ExcelWorksheet reportSheet;
    const int START_ROW_INDEX = 4;
    const int TOTAL_COLUMNS = 14;

    public async Task<ExcelPackage> Handle(ExportByLeagueRequest request, CancellationToken cancellationToken)
    {
        var provider = await GetProviderFromDb(request);
        var providerStatistic = CalculateProviderStatistic(provider);

        LoadTemplate(request.Template);
        BuildExcel(request.FromDate, request.ToDate, providerStatistic);
        ClearTemplate();
        return ExcelPackage;
    }

    private void BuildExcel(DateTime FromDate, DateTime ToDate, IList<ProviderStatisticInfoByLeague> model)
    {
        SetExcelTitle(FromDate, ToDate);
        reportSheet.ApplyVertical(model, START_ROW_INDEX);
        SetColorRow(model);
        SetGrandTotal(model);
        reportSheet.Cells[3, 1].Value = "League";
        reportSheet.Column(1).AutoFit();
        reportSheet.View.FreezePanes(START_ROW_INDEX, 2);
    }

    private void SetGrandTotal(IList<ProviderStatisticInfoByLeague> model)
    {
        if (model == null || model.Count == 0) return;
        var totalRow = START_ROW_INDEX + model.Count;
        const string COLOR_HEX = "#e6e8e9";
        Color color = ColorTranslator.FromHtml(COLOR_HEX);

        reportSheet.Cells[totalRow, 1].Value = "Grand Total";
        reportSheet.Cells[totalRow, 1].Style.Font.Bold = true;
        reportSheet.Cells[totalRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

        var grandTotalRange = reportSheet.Cells[totalRow, 1, totalRow, TOTAL_COLUMNS];
        grandTotalRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
        grandTotalRange.Style.Fill.BackgroundColor.SetColor(color);
        grandTotalRange.Style.Font.Bold = true;

        for (var col = 2; col <= TOTAL_COLUMNS; col++)
        {
            var colLetter = ExcelCellAddress.GetColumnLetter(col);
            var formula = $"SUM({colLetter}{START_ROW_INDEX}:{colLetter}{totalRow - 1})";
            reportSheet.Cells[totalRow, col].Formula = formula;
            reportSheet.Cells[totalRow, col].Style.Font.Bold = true;
            reportSheet.Cells[totalRow, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        }
    }

    private void SetExcelTitle(DateTime FromDate, DateTime ToDate)
    {
        var formattedFromDate = FromDate.ToString("yyyy-MM-dd");
        var formattedToDate = ToDate.ToString("yyyy-MM-dd");

        var cellFromDate = reportSheet.Cells[1, 2];
        cellFromDate.Value = $"{formattedFromDate}";

        var cellToDate = reportSheet.Cells[2, 2];
        cellToDate.Value = $"{formattedToDate}";
    }

    private void SetColorRow(IList<ProviderStatisticInfoByLeague> model)
    {
        const string BORDER_COLOR_HEX = "#000";
        Color borderColor = ColorTranslator.FromHtml(BORDER_COLOR_HEX);

        for (var i = 0; i < model.Count + 1; i++)
        {
            var rowIndex = i + START_ROW_INDEX;
            ApplyRowStyle(rowIndex, borderColor, TOTAL_COLUMNS);
        }
    }

    private void ApplyRowStyle(int rowIndex, Color borderColor, int totalColumns)
    {
        var range = reportSheet.Cells[rowIndex, 1, rowIndex, totalColumns];
        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        range.Style.Fill.BackgroundColor.SetColor(Color.White);
        ApplyBorderStyle(range, borderColor);
        range.Style.Font.Bold = false;
    }

    private static void ApplyBorderStyle(ExcelRange range, Color borderColor)
    {
        var border = range.Style.Border;
        border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        border.Top.Color.SetColor(borderColor);
        border.Bottom.Color.SetColor(borderColor);
        border.Left.Color.SetColor(borderColor);
        border.Right.Color.SetColor(borderColor);
    }

    private void LoadTemplate(string template)
    {
        var templatePath = @$"{configuration.CurrentDirectory}{Path.DirectorySeparatorChar}App_Data{Path.DirectorySeparatorChar}Templates{Path.DirectorySeparatorChar}{template}";

        using var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read);
        ExcelPackage = new ExcelPackage(stream);
        reportSheet = ExcelPackage.Workbook.Worksheets.Copy(Const.Const.SHEET_NAME, Const.Const.SHEET_PROVIDER_STATISTIC_NAME);

    }

    private void ClearTemplate()
    {
        ExcelPackage.Workbook.Worksheets.Delete(Const.Const.SHEET_NAME);
    }

    private async Task<IList<ProviderStatisticDbByLeague>> GetProviderFromDb(ExportByLeagueRequest request)
    {
        return (await repository.FetchAsync<ProviderStatisticDbByLeague>(new SumProviderByLeagueCriteria
        {
            FromDate = request.FromDate,
            ToDate = request.ToDate
        })).ToList();
    }

    private static IList<ProviderStatisticInfoByLeague> CalculateProviderStatistic(IList<ProviderStatisticDbByLeague> providerSource)
    {
        var groupedByLeague = providerSource
            .GroupBy(x => x.OWPLeagueId);

        var providerStatistic = groupedByLeague.Select(group =>
        {
            var agentLookup = group.ToDictionary(x => x.AgentId, x => x.Count);

            int GetCount(int agentId) => agentLookup.TryGetValue(agentId, out var count) ? count : 0;

            var gv365 = GetCount(AgentIds._365gv);
            var betradar = GetCount(AgentIds.Betradar);
            var bg = GetCount(AgentIds.Bg);
            var fc = GetCount(AgentIds.Fc);
            var img = GetCount(AgentIds.Img);
            var rb2 = GetCount(AgentIds.Rb2);
            var rts = GetCount(AgentIds.Rts);
            var slr = GetCount(AgentIds.Slr);
            var woo = GetCount(AgentIds.Woo);
            var streaming = GetCount(AgentIds.Streaming);
            var cancel = GetCount(AgentIds.Cancel);

            var totalMatches = gv365 + betradar + bg + fc + img + rb2 + rts + slr + woo + streaming;
            var total = totalMatches + cancel;

            return new ProviderStatisticInfoByLeague
            {
                _365Gv = gv365,
                Betradar = betradar,
                Bg = bg,
                Fc = fc,
                Img = img,
                Rb2 = rb2,
                Rts = rts,
                Slr = slr,
                Woo = woo,
                Streaming = streaming,
                CancelMatches = cancel,
                TotalMatches = totalMatches,
                Total = total,
                LeagueOrDay = group.First().LeagueName
            };
        }).OrderBy(x => x.LeagueOrDay)
        .ToList();

        return providerStatistic;
    }
}