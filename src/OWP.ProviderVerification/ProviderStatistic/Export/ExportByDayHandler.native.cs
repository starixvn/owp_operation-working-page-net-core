
using Epplus.Mapper.Extensions;
using OfficeOpenXml.Style;
using OWP.Constants;
using OWP.ProviderVerification.ProviderStatistic.Criteria;
using OWP.ProviderVerification.ProviderStatistic.Models;
using System.Drawing;

namespace OWP.ProviderVerification.ProviderStatistic.Export;

public record ExportByDayRequest(DateTime FromDate, DateTime ToDate, string Template) : Query<ExcelPackage>;

public class ExportByDayHandler(ConfigurationService configuration, IDynamicRepository repository) : QueryHandler<ExportByDayRequest, ExcelPackage>
{
    private ExcelPackage ExcelPackage { get; set; }
    private ExcelWorksheet reportSheet;
    const int START_ROW_INDEX = 4;
    const int TOTAL_COLUMNS = 14;

    public async Task<ExcelPackage> Handle(ExportByDayRequest request, CancellationToken cancellationToken)
    {
        var provider = await GetProviderFromDb(request);
        var providerStatistic= CalculateProviderStatistic(provider);

        LoadTemplate(request.Template);
        BuildExcel(request.FromDate, request.ToDate, providerStatistic);
        ClearTemplate();
        return ExcelPackage;
    }

    private void BuildExcel(DateTime FromDate, DateTime ToDate, IList<ProviderStatisticInfoByDay> model)
    {
        SetExcelTitle(FromDate, ToDate);
        reportSheet.ApplyVertical(model, START_ROW_INDEX);
        SetColorRow(model);
        SetGrandTotal(model);
        reportSheet.Cells[3, 1].Value = "Day";
    }

    private void SetGrandTotal(IList<ProviderStatisticInfoByDay> model)
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

    private void SetColorRow(IList<ProviderStatisticInfoByDay> model)
    {
        const string BORDER_COLOR_HEX = "#000";
        Color borderColor = ColorTranslator.FromHtml(BORDER_COLOR_HEX);

        for (int i = 0; i < model.Count + 1; i++)
        {
            int rowIndex = i + START_ROW_INDEX;
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

    private async Task<IList<ProviderStatisticDbByDay>> GetProviderFromDb(ExportByDayRequest request)
    {
        return (await repository.FetchAsync<ProviderStatisticDbByDay>(new SumProviderByDayCriteria
        {
            FromDate = request.FromDate,
            ToDate = request.ToDate
        })).ToList();
    }

    private static IList<ProviderStatisticInfoByDay> CalculateProviderStatistic(IList<ProviderStatisticDbByDay> providerSource)
    {
        var providerMapEventDate = providerSource
            .GroupBy(x => x.EventDate)
            .ToDictionary(x => x.Key, x => x);

        var providerStatistic = providerMapEventDate.Select(provider =>
            {
                var agentCounts = provider.Value.ToDictionary(x => x.AgentId, x => x.Count);

                int GetCount(int agentId) => agentCounts.TryGetValue(agentId, out var count) ? count : 0;

                var _365gv = GetCount(AgentIds._365gv);
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

                var totalMatches = _365gv + betradar + bg + fc + img + rb2 + rts + slr + woo + streaming;
                var total = totalMatches + cancel;

                return new ProviderStatisticInfoByDay
                {
                    _365Gv = _365gv,
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
                    EventDate = provider.Key,
                    LeagueOrDay = provider.Key.ToString("MMM-dd (ddd)")
                };
            })
            .OrderBy(p => p.EventDate)
            .ToList();

        return providerStatistic;
    }
}