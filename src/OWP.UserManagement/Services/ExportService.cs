namespace OWP.UserManagement.Services;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OWP.Models;

public interface ExportService
{
    void ExportToExcel(Stream stream, IEnumerable<User> users, DateTime date);
}

public class ExportServiceImp : ExportService
{
    private readonly ModelService modelService;

    private const string TextYes = "Yes";
    private const string TextNo = "No";

    public ExportServiceImp(ModelService modelService)
    {
        this.modelService = modelService;
    }

    private static void SetStyleHeaderColumn(int row, int col, string value, ExcelWorksheet worksheet)
    {
        const int redColor = 149;
        const int greenColor = 196;
        const int blueColor = 236;
        worksheet.Cells[row, col].Value = value;
        worksheet.Cells[row, col].Style.Font.Bold = true;
        worksheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
        worksheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(redColor, greenColor, blueColor));
    }

    private static void SetStyleHeader(ExcelWorksheet worksheet, IEnumerable<UserColumnModel> columns)
    {
        const int row = 1;
        var colIndex = 1;

        SetStyleHeaderColumn(row, colIndex, "No", worksheet);

        foreach (var item in columns)
        {
            var col = colIndex + 1;
            SetStyleHeaderColumn(row, col, item.HeaderText, worksheet);
            colIndex++;
        }
        worksheet.View.FreezePanes(2, 1);
    }

    private static string GetCellValue(User user, string propName)
    {
        var propertyInfo = user.GetType().GetProperty(propName);
        if (propertyInfo == null || propertyInfo.GetValue(user, null) == null)
        {
            return string.Empty;
        }

        var value = propertyInfo.GetValue(user, null);
        if (value is bool v)
        {
            return v ? TextYes : TextNo;
        }

        if (value is string
        && string.Equals(propName, "Pickup")
        && string.Equals(value, "none"))
        {
            return string.Empty;
        }
        return value.ToString();

    }

    private static void SetValueForRow(ExcelWorksheet worksheet, IEnumerable<UserColumnModel> columns, IEnumerable<User> users)
    {
        var row = 2;
        var rowIndex = 1;

        foreach (var item in users)
        {
            var col = 1;

            worksheet.Cells[row, col].Value = rowIndex;
            col++;

            foreach (var column in columns)
            {
                var displayValue = GetCellValue(item, column.Name);
                worksheet.Cells[row, col].Value = displayValue;

                col++;
            }

            row++;
            rowIndex++;
        }
    }

    public void ExportToExcel(Stream stream, IEnumerable<User> users, DateTime date)
    {
        var listColumns = modelService.BuildUserColumnModel();

        using (var xlPackage = new ExcelPackage(stream))
        {
            var worksheet = xlPackage.Workbook.Worksheets.Add($"{date.ToString("yyyy-MM-dd")} (Total {users.Count()} users)");
            SetStyleHeader(worksheet, listColumns);
            SetValueForRow(worksheet, listColumns, users);
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            xlPackage.SaveAs(stream);
        }
    }
}