

using MediatR;
using OfficeOpenXml;
using Epplus.Mapper.Extensions;
using OWP.Kpi.LA.Services;
using OWP.Services;
using AutoMapper;
using OWP.Kpi.LA.ViewModels;
using OfficeOpenXml.Style;
using System.Drawing;
using OWP.Kpi.LA.Helper;
using OWP.Kpi.LA.Constant;

namespace OWP.Kpi.LA.Handlers;
public class ExportKpiRequest  :IRequest<ExcelPackage>
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }
}
public class ExportKpiHandler : IRequestHandler<ExportKpiRequest, ExcelPackage>, IDisposable
{
    private ExcelPackage excelPackage;

    private ExcelWorksheet reportSheet;

    private readonly KpiService kpiService;

    private readonly ConfigurationService configurationService;
    
    private readonly IMapper mapper;

    public ExportKpiHandler(KpiService kpiService,  ConfigurationService configurationService, IMapper mapper)
    {
        this.kpiService = kpiService;
        this.configurationService = configurationService;
        this.mapper = mapper;
    }

    public async Task<ExcelPackage> Handle(ExportKpiRequest request, CancellationToken cancellationToken)
    {
        var templatePath = GetReportTemplate();
        LoadTemplate(templatePath);
        await  BuildExcelBody(request);
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
        var kpiInfo = await kpiService.BuildKpiInfo(request.FromDate, request.ToDate,isGap);
        var kpiReport =  mapper.Map<IList<KpiInfoReport>>(kpiInfo);
        reportSheet.ApplyVertical(kpiReport, 3);
        SetStyleTemplate();
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
    private void SetStyleTemplate()
    {
        var dimension = reportSheet.Dimension;
        var end = dimension.End;
        for (int row = 3; row <= end.Row; row++)
        {
            for (int col = 1; col <= dimension.End.Column; col++)
            {
                var cell = reportSheet.Cells[row, col];
                if (cell.Value is int numericValue and < 0 )
                {
                    reportSheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cell.Style.Font.Color.SetColor(Color.Red);
                    cell.Style.Fill.BackgroundColor.SetColor(Color.White);
                    cell.Value = -numericValue;
                }
                if (col == 3 && cell.Value?.ToString() == "Fail")
                {
                    reportSheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cell.Style.Font.Color.SetColor(Color.Red);
                    cell.Style.Fill.BackgroundColor.SetColor(Color.White);
                }
            }
        }
    }

    private void DeleteColumnTemplate()
    {
        List<int> columnsToDelete = ReportConstant.DeleteColumns.ToList();

        for (int i = columnsToDelete.Count - 1; i >= 0; i--)
        {
            int column = columnsToDelete[i];
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