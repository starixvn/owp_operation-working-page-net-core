using Epplus.Mapper.Extensions;
using OfficeOpenXml;
using OWP.AutoAssignment.Constant;
using OWP.AutoAssignment.Models.InternalApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.AutoAssignment.Services.InternalApi
{
    public interface ExcelPackageCreatorService
    {
        ExcelPackage CreateExcelPackage(string templatePath, ExportExcelModel model);
    }

    public class ExcelPackageCreatorServiceImp : ExcelPackageCreatorService
    {
        private ExcelPackage excelPackage { get; set; }

        private ExcelWorksheet reportSheet;

        public ExcelPackage CreateExcelPackage(string templatePath, ExportExcelModel model)
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

        private void BuildReportBody(ExportExcelModel model)
        {
            reportSheet.ApplyModel(model);
            reportSheet.ApplyVertical(model.exportMatchModels, AutoConstant.StartRowIndex);
        }

        private void ClearTemplate()
        {
            excelPackage.Workbook.Worksheets.Delete(AutoConstant.TemplateName);
        }
    }
}