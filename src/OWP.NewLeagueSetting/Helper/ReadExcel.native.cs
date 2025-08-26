using System.ComponentModel.DataAnnotations;

namespace OWP.NewLeagueSetting.Helper;

public static class ReadExcel
{
    public static IList<LeagueSettingExcelInfo> LoadData(Stream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);
        using var package = new ExcelPackage(stream);

        var worksheet = package.Workbook.Worksheets.First();

        var props = new LeagueSettingExcelInfo()
            .GetType()
            .GetProperties()
            .ToList();

        const int startRow = 2;
        var end = worksheet.Dimension.End;
        var cells = new List<ExcelRangeBase>();

        for (var row = startRow; row <= end.Row; row++)
        {
            for (var col = 1; col <= end.Column; col++)
            {
                cells.Add(worksheet.Cells[row, col]);
            }
        }

        var groups = cells
            .GroupBy(cell => cell.Start.Row)
            .ToList();

        // Get column names from the header row (row 2)
        var headerRow = worksheet.Cells[2, 1, 2, end.Column];
        var colNames = new List<dynamic>();
        
        for (int col = 1; col <= end.Column; col++)
        {
            var headerValue = headerRow[2, col].Value?.ToString();
            if (!string.IsNullOrWhiteSpace(headerValue))
            {
                colNames.Add(new { Name = headerValue, index = col - 1 });
            }
        }

        var rowValues = groups
            .Skip(1)
            .Select(cg => cg.Select(c => new
            {
                c?.Value,
                note = c?.Comment?.Text
            }).ToList())
            ;

        var leagueSetting = rowValues
            .Select(row =>
            {
                var settingInfo = new LeagueSettingExcelInfo();
                colNames.ForEach(colName =>
                {
                    if (colName.index < row.Count)
                    {
                        var value = row[colName.index];
                        var prop = props.FirstOrDefault(p => 
                            p.Name.Equals(colName.Name, StringComparison.OrdinalIgnoreCase) ||
                            p.GetCustomAttributes(typeof(DisplayAttribute), false)
                                .Cast<DisplayAttribute>()
                                .Any(attr => attr.Name?.Equals(colName.Name, StringComparison.OrdinalIgnoreCase) == true));
                        
                        if (prop != null)
                        {
                            prop.SetValue(settingInfo, value?.Value?.ToString());
                        }
                    }
                });

                return settingInfo;
            });

        var result = leagueSetting.Where(l => !string.IsNullOrWhiteSpace(l.League)).ToList();
        return result;
    }

    public static void CorrectSpecialSpreadValue(IList<LeagueSettingExcelInfo> excelInfo)
    {
        foreach (var info in excelInfo)
        {
            var regex = Pattern.SpreadSpecialRegex();
            if (!string.IsNullOrWhiteSpace(info.FTSpread) && regex.IsMatch(info.FTSpread))
            {
                if (decimal.TryParse(info.FTSpread, out var spread))
                {
                    info.FTSpread = $"{(int)(spread * 100)}%";
                }
            }
        }
    }
}