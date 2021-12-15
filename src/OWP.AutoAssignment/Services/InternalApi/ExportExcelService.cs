using Epplus.Mapper.Extensions;
using OfficeOpenXml;
using OWP.AutoAssignment.Constant;
using OWP.AutoAssignment.Models.InternalApi;
using System.IO;

namespace OWP.AutoAssignment.Services.InternalApi
{
    public interface ExportExcelService
    {
        ExcelPackage CreateExcelPackage(string templatePath, ExportModel model);
    }

    public class ExportExcelServiceImp : ExportExcelService
    {
        private ExcelPackage excelPackage { get; set; }

        private ExcelWorksheet reportSheet;

        public ExcelPackage CreateExcelPackage(string templatePath, ExportModel model)
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
                reportSheet = excelPackage.Workbook.Worksheets.Copy(AutoConstant.TemplateName, AutoConstant.SheetName);
            }
        }

        public void BuildReportBody(ExportModel model)
        {
            reportSheet.ApplyModel(model.Date);
            reportSheet.ApplyVertical(model.exportMatchModels, AutoConstant.StartRowIndex);
        }

        public void ClearTemplate()
        {
            excelPackage.Workbook.Worksheets.Delete(AutoConstant.TemplateName);
        }
    }
}