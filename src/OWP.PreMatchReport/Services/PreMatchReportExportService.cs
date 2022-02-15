using System.Globalization;
using AutoMapper;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OWP.Extensions;
using OWP.PreMatchReport.Constants;
using OWP.PreMatchReport.Models;
using OWP.Services;

namespace OWP.PreMatchReport.Services;

public interface PreMatchReportExportService
{
    Task<byte[]> Export(QueryModel query);
}

public class PreMatchReportExportServiceImp : PreMatchReportExportService
{
    private readonly ConfigurationService configurationService;
    private readonly GetPreMatchReportService preMatchReportService;

    public PreMatchReportExportServiceImp(ConfigurationService configurationService, GetPreMatchReportService preMatchReportService)
    {
        this.configurationService = configurationService;
        this.preMatchReportService = preMatchReportService;
    }

    private FileInfo GetReportTemplate(string fileName)
    {
        var templatePath = @$"App_Data/Templates/{fileName}";
        var absoluteTemplatePath = $"{configurationService.CurrentDirectory}{Path.DirectorySeparatorChar}{templatePath}";

        return new FileInfo(absoluteTemplatePath);
    }

    public async Task<byte[]> Export(QueryModel query)
    {
        var file = GetReportTemplate(ReportConst.ReportTemplate);

        if (!file.Exists)
        {
            throw new FileNotFoundException(ReportConst.ReportTemplate);
        }

        var records = await GetDataExportPreMatch(query);

        using var package = new ExcelPackage(file);

        var worksheet = package.Workbook.Worksheets[0];
        worksheet.Cells["A1"].Value = $"Date: {query.FromDate.ToFormatVn()} To {query.ToDate.ToFormatVn()}";
        worksheet.Cells["A3"].LoadFromCollection(records);
        Visualize(worksheet, records);
        return await package.GetAsByteArrayAsync();
    }

    private async Task<List<ExportPreMatchModel>> GetDataExportPreMatch(QueryModel query)
    {
        var result = await preMatchReportService.GetReports(query);

        return result.ToList();
    }

    private void Visualize(ExcelWorksheet worksheet, List<ExportPreMatchModel> records)
    {
        var rowEnd = records.Count + 2;
        var range = $"A1:BC{rowEnd}";

        worksheet.Cells[range].Style.Border.Top.Style = ExcelBorderStyle.Thin;
        worksheet.Cells[range].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        worksheet.Cells[range].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        worksheet.Cells[range].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

        var rangeData = records.Count == 0 ? "B3:BC3" : $"B3:BC{rowEnd}";

        worksheet.Cells[rangeData].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
    }
}