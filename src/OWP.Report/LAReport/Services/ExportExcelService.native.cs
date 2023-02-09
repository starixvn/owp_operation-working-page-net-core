namespace OWP.Report.LAReport.Services;

using System.IO;
using Constant;
using Epplus.Mapper.Extensions;
using Models;
using OfficeOpenXml;

public interface ExportExcelService
{
    ExcelPackage CreateExcelPackage(string templatePath, LAExcelModel model);
}

public class ExportExcelServiceImp : ExportExcelService
{
    private ExcelPackage excelPackage { get; set; }

    private ExcelWorksheet reportSheet;

    public void LoadTemplate(string templatePath)
    {
        using (var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
        {
            excelPackage = new ExcelPackage(stream);
            reportSheet = excelPackage.Workbook.Worksheets.Copy(ReportConst.TemplateName, ReportConst.SheetName);
        }
    }

    public void ClearTemplate()
    {
        excelPackage.Workbook.Worksheets.Delete(ReportConst.TemplateName);
    }

    public ExcelPackage CreateExcelPackage(string templatePath, LAExcelModel model)
    {
        LoadTemplate(templatePath);
        BuildLAReportBody(model);
        ClearTemplate();
        return excelPackage;
    }

    public void BuildLAReportBody(LAExcelModel model)
    {
        reportSheet.ApplyModel(model.FileInfo);
        reportSheet.ApplyVertical(model.ExcelModels, ReportConst.StartRowIndex);
    }
}