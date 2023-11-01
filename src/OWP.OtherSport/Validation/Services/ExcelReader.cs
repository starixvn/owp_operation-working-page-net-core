using OfficeOpenXml;
using OWP.OtherSport.Validation.Extensions;
using OWP.OtherSport.Validation.Models;
using System.Text.RegularExpressions;
// ReSharper disable LoopCanBeConvertedToQuery

namespace OWP.OtherSport.Validation.Services;

public interface ExcelReader
{
    UploadingDataFromExcel Read(Stream stream, DateTime fromDate, DateTime toDate);
}

public class ExcelReaderImp : ExcelReader
{
    private const int StartRow = 2;

    private static readonly Regex otherTraderRegex = new(@"#[^\s#]+", RegexOptions.IgnoreCase);

    public UploadingDataFromExcel Read(Stream stream, DateTime fromDate, DateTime toDate)
    {
        stream.Seek(0, SeekOrigin.Begin);

        using var excelPackage = new ExcelPackage(stream);
        using var worksheet = excelPackage.Workbook.Worksheets.First();
        var assignments = worksheet.ReadAllAssignments(StartRow).ToList();
        var validAssignments = GetValidAssignments(assignments, fromDate, toDate);

        return new UploadingDataFromExcel
        {
            Assignments = validAssignments,
            Traders = GetTraders(validAssignments)
        };
    }

    public static List<string> GetTraders(List<AssignmentModelInExcel> assignments)
    {
        var mainTraders = GetTraderWithNote(assignments, "Trader");
        var otherTraders = GetTraderWithNote(assignments, "OtherTrader");
        return mainTraders.Union(otherTraders)
            .Where(t => !string.IsNullOrEmpty(t))
            .Distinct()
            .ToList();
    }

    private static IEnumerable<string> GetTraderWithNote(IEnumerable<AssignmentModelInExcel> assignments, string type)
    {
        var otherTraderColumns = assignments.Select(a => GetTradersWithType(a, type))
            .SelectMany(l => l)
            .Where(b => !string.IsNullOrEmpty(b))
            .ToArray();

        return otherTraderColumns
            .Select(ExtractOtherTrader)
            .Distinct()
            .ToList();
    }

    private static IEnumerable<string> GetTradersWithType(AssignmentModelInExcel assignment, string type)
    {
        var properties = typeof(AssignmentModelInExcel).GetProperties();
        var result = new List<string>();

        foreach (var prop in properties) // NOSONAR
        {
            if (prop.Name.StartsWith(type, StringComparison.CurrentCultureIgnoreCase))
            {
                result.Add(prop.GetValue(assignment, null).ToString());
            }
        }

        return result;
    }

    public static string ExtractOtherTrader(string text)
    {
        var matches = otherTraderRegex.Matches(text);
        return matches.FirstOrDefault()?.Value.Remove(0, 1);
    }

    private static List<AssignmentModelInExcel> GetValidAssignments(IEnumerable<AssignmentModelInExcel> assignment, DateTime fromDate, DateTime toDate)
    {
        var fromDateFormat = fromDate.AddHours(12);
        var toDateFormat = toDate.AddDays(1).AddHours(11).AddMinutes(59).AddSeconds(59);
        return assignment
            .Where(x => CovertDateTime(x.DateTime) >= fromDateFormat && CovertDateTime(x.DateTime) <= toDateFormat)
            .ToList();
    }

    public static DateTime CovertDateTime(string dateTimeInString)
    {
        var doubleDatetime = double.TryParse(dateTimeInString, out var number) ? number : 0;
        return DateTime.FromOADate(doubleDatetime);
    }
}