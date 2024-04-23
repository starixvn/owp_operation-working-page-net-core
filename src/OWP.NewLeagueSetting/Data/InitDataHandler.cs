
namespace OWP.NewLeagueSetting.Data;

public record InitDataRequest(Stream Stream) : IRequest<IList<LeagueSettingExcelInfo>>;
public class InitDataHandler : IRequestHandler<InitDataRequest, IList<LeagueSettingExcelInfo>>
{
    public async Task<IList<LeagueSettingExcelInfo>> Handle(InitDataRequest request, CancellationToken cancellationToken)
    {
        var stream = request.Stream;
        stream.Seek(0, SeekOrigin.Begin);
        using var package = new ExcelPackage(stream);

        var sheet = package.Workbook.Worksheets[3];

        return LoadData(sheet);
    }

    private static IList<LeagueSettingExcelInfo> LoadData(ExcelWorksheet worksheet)
    {
        var props = (new LeagueSettingExcelInfo())
            .GetType()
            .GetProperties()
            .ToList();

        const int startRow = 8;
        var end = worksheet.Dimension.End;
        var cells = new List<ExcelRangeBase>();

       
        for (var r = startRow; r <= end.Row; r++)
            for (var c = 2; c <= end.Column; c++)
                cells.Add(worksheet.Cells[r, c]);

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
                    var val = row[colName.index];
                    var prop = props.First(p => p.Name == colName.Name);
                    var propComment = props.First(p => p.Name == colName.Name + "Note");
                    prop.SetValue(settingInfo, val?.Value?.ToString());
                    propComment.SetValue(settingInfo, val?.note);
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