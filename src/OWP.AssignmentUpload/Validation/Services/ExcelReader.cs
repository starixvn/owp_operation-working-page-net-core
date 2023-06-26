using OfficeOpenXml;
using OWP.AssignmentUpload.Validation.Constants;
using OWP.AssignmentUpload.Validation.Extensions;
using OWP.AssignmentUpload.Validation.Models;
using System.Text.RegularExpressions;

namespace OWP.AssignmentUpload.Validation.Services;

public interface ExcelReader
{
    UploadingDataFromExcel Read(Stream stream);
}

public class ExcelReaderImp : ExcelReader
{
    private const int StartRow = 3;

    private static readonly Regex otherTraderRegex = new(@"#[^\s#]+", RegexOptions.IgnoreCase);

    public UploadingDataFromExcel Read(Stream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);

        using var package = new ExcelPackage(stream);
        var sheet = package.Workbook.Worksheets.First();
        var assignments = sheet.ReadAllAssignments(StartRow).ToList();
        var eventDate = sheet.GetDateTime();

        return new UploadingDataFromExcel
        {
            EventDate = eventDate,
            Assignments = assignments,
            Traders = GetTraders(assignments)
        };
    }

    public static List<string> GetTraders(List<AssignmentModelInExcel> assignments)
    {
        var mainTraders = GetMainTraders(assignments);
        var otherTraders = GetOtherTraders(assignments);
        return mainTraders.Union(otherTraders)
            .Where(t => !string.IsNullOrEmpty(t))
            .Select(t => t.Replace(",", string.Empty))
            .Distinct()
            .ToList();
    }

    public static List<string> GetMainTraders(List<AssignmentModelInExcel> assignments)
    {
        return assignments.Select(a => new[]
            {
                a.Trader10MinsName,
                a.Trader15MinsName,
                a.Trader1H1X2Name,
                a.Trader1HHDPName,
                a.Trader1HOUName,
                a.Trader1X2Name,
                a.TraderAIOName,
                a.TraderFMName,
                a.TraderHDPName,
                a.TraderOUName,
                a.TraderLAName
            })
            .SelectMany(l => l)
            .Where(b => !string.IsNullOrEmpty(b))
            .Distinct()
            .ToList();
    }

    public static List<string> GetOtherTraders(List<AssignmentModelInExcel> assignments)
    {
        var otherTraderColumns = assignments.Select(a => new[]
            {
                a.OtherTraders10Mins,
                a.OtherTraders15Mins,
                a.OtherTraders1H1X2,
                a.OtherTraders1HHDP,
                a.OtherTraders1HOU,
                a.OtherTraders1X2,
                a.OtherTradersAIO,
                a.OtherTradersFM,
                a.OtherTradersHDP,
                a.OtherTradersOU,
                a.OtherTradersLA
            })
            .SelectMany(l => l)
            .Where(b => !string.IsNullOrEmpty(b))
            .ToArray();

        return otherTraderColumns
            .Select(ExtractOtherTraders)
            .SelectMany(l => l)
            .Distinct()
            .ToList();
    }

    public static List<string> ExtractOtherTraders(string text)
    {
        const int maxLength = EventUploadingMaxLength.TraderName;
        var parts = text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        return parts.Select(p =>
            {

                var matches = otherTraderRegex.Matches(p);
                return matches.Select(s => s.Value.Remove(0, 1)).FirstOrDefault();
            })
            .Where(n => !string.IsNullOrEmpty(n))
            .Select(l => l.Length <= maxLength ? l : l[..maxLength])
            .ToList();
    }
}