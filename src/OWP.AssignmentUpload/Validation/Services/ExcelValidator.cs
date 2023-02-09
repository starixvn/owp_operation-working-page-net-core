namespace OWP.AssignmentUpload.Validation.Services;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Exceptions;
using Extensions;
using OfficeOpenXml;

public interface ExcelValidator
{
    void Validate(Stream stream);
}

public class ExcelValidatorImp : ExcelValidator
{
    public void Validate(Stream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);

        using (var package = new ExcelPackage(stream))
        {
            var sheet = package.Workbook.Worksheets.First();
            ValidateHeaders(sheet);
            ValidateCellValues(sheet);
        }
    }

    private static void ValidateHeaders(ExcelWorksheet sheet)
    {
        var headerMap = sheet.GetHeaderMap();

        var anyInvalidHeaderColumnIndex = headerMap.Any(i => i.Value <= 0);
        if (anyInvalidHeaderColumnIndex)
        {
            throw new InvalidExcelFormatException();
        }
    }

    private static void ValidateCellValues(ExcelWorksheet sheet)
    {
        var errorCellAddresses = new List<string>();

        for (var i = 3; i <= sheet.Dimension.End.Row; i++)
        {
            for (var j = 1; j < sheet.Dimension.End.Column; j++)
            {
                var cellValue = sheet.Cells[i, j].Value;
                if (cellValue is ExcelErrorValue)
                {
                    var excelAddress = new ExcelAddress(i, j, i, j).Address;
                    errorCellAddresses.Add($"{excelAddress.Substring(0, 2)}-{i}");
                }
            }
        }

        if (errorCellAddresses.Any())
        {
            var message = $"資料不正確, 請查看列表: {string.Join(",", errorCellAddresses)}";
            throw new InvalidExcelCellFormatException(message);
        }
    }
}