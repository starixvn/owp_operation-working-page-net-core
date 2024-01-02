

using AutoMapper;
using Epplus.Mapper.Extensions;
using MediatR;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OWP.Kpi.LA.Constant;
using OWP.Kpi.LA.Helper;
using OWP.Kpi.LA.Models;
using OWP.Kpi.LA.Services;
using OWP.Kpi.LA.ViewModels;
using OWP.Services;
using System.Drawing;

namespace OWP.Kpi.LA.Handlers;
public class ExportKpiRequest : QueryModel, IRequest<ExcelPackage>
{

}
public class ExportKpiHandler : IRequestHandler<ExportKpiRequest, ExcelPackage>, IDisposable
{
    private ExcelPackage excelPackage;

    private ExcelWorksheet reportSheet;

    private readonly KpiService kpiService;

    private readonly ConfigurationService configurationService;

    private readonly IMapper mapper;

    public ExportKpiHandler(KpiService kpiService, ConfigurationService configurationService, IMapper mapper)
    {
        this.kpiService = kpiService;
        this.configurationService = configurationService;
        this.mapper = mapper;
    }

    public async Task<ExcelPackage> Handle(ExportKpiRequest request, CancellationToken cancellationToken)
    {
        var templatePath = GetReportTemplate();
        LoadTemplate(templatePath);
        await BuildExcelBody(request);
        return await Task.FromResult(excelPackage);
    }

    private void LoadTemplate(string templatePath)
    {
        using var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read);
        excelPackage = new ExcelPackage(stream);
        reportSheet = excelPackage.Workbook.Worksheets[0];
    }

    private async Task BuildExcelBody(ExportKpiRequest request)
    {
        var isGap = CalculateGapHelper.IsGap(request.FromDate, request.ToDate);
        var kpiInfo = await kpiService.BuildKpiInfo(request, isGap);
        var kpiReport = mapper.Map<IList<KpiInfoReport>>(kpiInfo);
        reportSheet.ApplyVertical(kpiReport, 3);

        SetStyleTemplate(kpiInfo);
        if (!isGap)
        {
            DeleteColumnTemplate();
        }
    }

    private string GetReportTemplate()
    {
        const string templatePath = @"App_Data/Templates/LAKpiReportTemplate.xlsx";
        return $"{configurationService.CurrentDirectory}{Path.DirectorySeparatorChar}{templatePath}";
    }

    private void SetStyleTemplate(IList<KpiInfoViewModel> kpiInfos)
    {
        var dimension = reportSheet.Dimension;
        var endRow = kpiInfos.Count + 2;
        for (var row = ReportConstant.StartRow; row <= endRow; row++)
        {
            for (var col = 1; col <= dimension.End.Column; col++)
            {
                var cell = reportSheet.Cells[row, col];
                if (cell.Value is double numericValue and < 0)
                {
                    reportSheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.None;
                    cell.Style.Font.Color.SetColor(Color.Red);
                    cell.Value = -numericValue;
                }

                if (col == ReportConstant.ProgressColumn)
                {
                    FillProgressCellStyle(kpiInfos, cell, row);
                }
            }
        }
    }

    private void FillProgressCellStyle(IList<KpiInfoViewModel> kpiInfos, ExcelRange cell, int row)
    {
        const int colIndex = ReportConstant.ProgressColumn;
        if (cell.Value?.ToString() == "Fail")
        {
            reportSheet.Cells[row, colIndex].Style.Fill.PatternType = ExcelFillStyle.None;
            cell.Style.Font.Color.SetColor(Color.Red);
        }

        if (!kpiInfos[row - ReportConstant.StartRow].IsAlmostWinTarget)
        {
            return;
        }

        reportSheet.Cells[row, colIndex].Style.Fill.PatternType = ExcelFillStyle.Solid;
        reportSheet.Cells[row, colIndex].Style.Fill.BackgroundColor.SetColor(ReportConstant.AlmostArchivedTargetColor);
        reportSheet.Cells[row, colIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.DarkGray);
    }

    private void DeleteColumnTemplate()
    {
        var columnsToDelete = ReportConstant.DeleteColumns.ToList();

        for (var i = columnsToDelete.Count - 1; i >= 0; i--)
        {
            var column = columnsToDelete[i];
            reportSheet.DeleteColumn(column);
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