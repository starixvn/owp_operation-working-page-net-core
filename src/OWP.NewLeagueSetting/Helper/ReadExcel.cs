namespace OWP.NewLeagueSetting.Helper;

public static class ReadExcel
{
    public static IList<LeagueSettingExcelInfo> LoadData(Stream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);
        using var package = new ExcelPackage(stream);

        var worksheet = package.Workbook.Worksheets[3];

        var props = new LeagueSettingExcelInfo()
            .GetType()
            .GetProperties()
            .ToList();

        const int startRow = 8;
        var end = worksheet.Dimension.End;
        var cells = new List<ExcelRangeBase>();


        for (var row = startRow; row <= end.Row; row++)
            for (var col = 2; col <= end.Column; col++)
                cells.Add(worksheet.Cells[row, col]);

        var groups = cells
            .GroupBy(cell => cell.Start.Row)
            .ToList();

        var colNames = groups
            .First()
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


        // TODO
        //var result = collection.Where(c => c.LeagueGroup is not null ||
        //                                   c.LeagueNote is not null ||
        //                                   c.LocalTvNote is not null ||
        //                                   c.RoundNote is not null ||
        //                                   c.LeagueDivisionNote is not null ||
        //                                   c.RemarkNote is not null ||
        //                                   c.FTSpreadNote is not null ||
        //                                   c.WD_HDPNote is not null ||
        //                                   c.WD_NO_HDPNote is not null ||
        //                                   c.WK_HDPNote is not null ||
        //                                   c.WK_NO_HDPNote is not null ||
        //                                   c.LstNoteNote is not null ||
        //                                   c.Remark2Note is not null).ToList();
        return leagueSetting.ToList();
    }

}