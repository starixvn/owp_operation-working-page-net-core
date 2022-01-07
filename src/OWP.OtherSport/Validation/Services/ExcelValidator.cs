namespace OWP.OtherSport.Validation.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Extensions;
    using OfficeOpenXml;
    using OWP.OtherSport.Validation.Constants;
    using OWP.OtherSport.Validation.Exceptions;

    public interface ExcelValidator
    {
        void Validate(Stream stream);
    }

    public class ExcelValidatorImp : ExcelValidator
    {
        public void Validate(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);

            using var excelPackage = new ExcelPackage(stream);
            using var worksheet = excelPackage.Workbook.Worksheets.First();
            ValidateDataLength(worksheet);
            ValidateHeaders(worksheet);
            ValidateCellValues(worksheet);
        }

        private static void ValidateDataLength(ExcelWorksheet sheet)
        {
            if ((sheet.Dimension.End.Row - 1) > ExcelDataLength.MaximumDataLength)
            {
                throw new DataLengthExceedsMaximumException();
            }
        }

        private static void ValidateHeaders(ExcelWorksheet sheet)
        {
            var headerMap = sheet.GetHeaderMap();

            if (headerMap.Any(i => i.Value <= 0))
            {
                throw new InvalidExcelFormatException();
            }
        }

        private static void ValidateCellValues(ExcelWorksheet sheet)
        {
            var errorCellAddresses = new List<string>();
            const int rowAddressLength = 2;

            for (var i = 2; i <= sheet.Dimension.End.Row; i++)
            {
                for (var j = 1; j < sheet.Dimension.End.Column; j++)
                {
                    var cellValue = sheet.Cells[i, j].Value;
                    if (cellValue is ExcelErrorValue)
                    {
                        var excelAddress = new ExcelAddress(i, j, i, j).Address;
                        errorCellAddresses.Add($"{excelAddress.Substring(0, rowAddressLength)}-{i}");
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
}