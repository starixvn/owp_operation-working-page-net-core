using OfficeOpenXml;
using OWP.AssignmentUpload.Validation.Exceptions;
using OWP.AssignmentUpload.Validation.Extensions;

namespace OWP.AssignmentUpload.Validation.Services;

public interface ExcelValidator
{
    void Validate(Stream stream);
}

public class ExcelValidatorImp : ExcelValidator
{
    private readonly string[] OptionalColumn = ["Agent for EST", "Remark for EST", "Remark by", "SOP"];

    public void Validate(Stream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);

        using var package = new ExcelPackage(stream);
        var sheet = package.Workbook.Worksheets.First();
        ValidateHeaders(sheet);
        ValidateCellValues(sheet);
    }

    private void ValidateHeaders(ExcelWorksheet sheet)
    {
        var headerMap = sheet.GetHeaderMap();

        var anyInvalidHeaderColumnIndex = headerMap.FirstOrDefault(i => i.Value <= 0 && !Array.Exists(OptionalColumn, l => string.Equals(l, i.Key)));
        if (anyInvalidHeaderColumnIndex.Key is not null)
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
                if (cellValue is not ExcelErrorValue)
                {
                    continue;
                }

                var excelAddress = new ExcelAddress(i, j, i, j).Address;
                errorCellAddresses.Add($"{excelAddress[..2]}-{i}");
            }
        }

        if (!errorCellAddresses.Any())
        {
            return;
        }

        var message = $"資料不正確, 請查看列表: {string.Join(",", errorCellAddresses)}";
        throw new InvalidExcelCellFormatException(message);
    }
}