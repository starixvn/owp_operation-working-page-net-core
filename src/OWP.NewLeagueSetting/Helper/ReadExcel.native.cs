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

        const int startRow = 8;
        var end = worksheet.Dimension.End;
        var cells = new List<ExcelRangeBase>();


        for (var row = startRow; row <= end.Row; row++)
        {
            for (var col = 2; col <= end.Column; col++)
            {
                cells.Add(worksheet.Cells[row, col]);
            }
        }

        var groups = cells
            .GroupBy(cell => cell.Start.Row)
            .ToList();

        var colNames = groups[0]
            .Select((cell, idx) => new { Name = cell.Value?.ToString(), index = idx })
            .Where(o => props.Select(p => p.Name).Contains(o.Name))
            .ToList();

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
                    var value = row[colName.index];
                    var prop = props.First(p => p.Name == colName.Name);
                    var propComment = props.First(p => p.Name == $"{colName.Name}Note");
                    prop.SetValue(settingInfo, value?.Value?.ToString());
                    propComment.SetValue(settingInfo, value?.note);
                });

                return settingInfo;
            });

        var result = leagueSetting.Where(l => !string.IsNullOrWhiteSpace(l.League)).ToList();
        return result;
    }

}