
namespace OWP.NewLeagueSetting.Data;

public record InitDataRequest(Stream Stream) : IRequest<Unit>;
public class InitDataHandler : IRequestHandler<InitDataRequest, Unit>
{
    public async Task<Unit> Handle(InitDataRequest request, CancellationToken cancellationToken)
    {
        var stream = request.Stream;
        stream.Seek(0, SeekOrigin.Begin);
        using var package = new ExcelPackage(stream);

        // TODO
        var sheet = package.Workbook.Worksheets[4];

        LoadData(sheet.Tables.First());
        return Unit.Value;
    }

    private static IList<LeagueSettingExcelInfo> LoadData(ExcelTable table)
    {
        var tprops = (new LeagueSettingExcelInfo())
            .GetType()
            .GetProperties()
            .ToList();

        var start = table.Address.Start;
        var end = table.Address.End;
        var cells = new List<ExcelRangeBase>();

        //Have to use for loops insteadof worksheet.Cells to protect against empties
        for (var r = start.Row; r <= end.Row; r++)
            for (var c = start.Column; c <= end.Column; c++)
                cells.Add(table.WorkSheet.Cells[r, c]);

        var groups = cells
            .GroupBy(cell => cell.Start.Row)
            .ToList();

        //Assume first row has the column names
        var colNames = groups
            .First()
            .Select((cell, idx) => new { Name = cell.Value.ToString(), index = idx })
            .Where(o => tprops.Select(p => p.Name).Contains(o.Name))
            .ToList();

        //Everything after the header is data
        var rowvalues = groups
            .Skip(1) //Exclude header
            .Select(cg => cg.Select(c => c.Value).ToList());

        //Create the collection container
        var collection = rowvalues
            .Select(row =>
            {
                var tnew = new LeagueSettingExcelInfo();
                colNames.ForEach(colName =>
                {
                    var val = row[colName.index];
                    var prop = tprops.First(p => p.Name == colName.Name);
                    prop.SetValue(tnew, val);
                });

                return tnew;
            });


        //Send it back
        return collection.ToList();
    }
}