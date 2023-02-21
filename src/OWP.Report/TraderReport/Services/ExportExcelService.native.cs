using Epplus.Mapper.Extensions;
using OfficeOpenXml;
using OWP.Report.Constant;
using OWP.Report.TraderReport.Models;

namespace OWP.Report.TraderReport.Services;

public interface ExportExcelService
{
    ExcelPackage CreateExcelPackage(string templatePath, ExcelModel model);
}

public class ExportExcelServiceImp : ExportExcelService
{
    private ExcelPackage excelPackage { get; set; }

    private ExcelWorksheet reportSheet;

    public ExcelPackage CreateExcelPackage(string templatePath, ExcelModel model)
    {
        LoadTemplate(templatePath);
        BuildReportBody(model);
        ClearTemplate();
        return excelPackage;
    }

    public void LoadTemplate(string templatePath)
    {
        using (var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
        {
            excelPackage = new ExcelPackage(stream);
            reportSheet = excelPackage.Workbook.Worksheets.Copy(ReportConst.TemplateName, ReportConst.SheetName);
        }
    }

    public void BuildReportBody(ExcelModel model)
    {
        reportSheet.ApplyModel(model.FileInfo);
        reportSheet.ApplyVertical(model.ExcelModels, ReportConst.StartRowIndex);
    }

    public void ClearTemplate()
    {
        excelPackage.Workbook.Worksheets.Delete(ReportConst.TemplateName);
    }        
}