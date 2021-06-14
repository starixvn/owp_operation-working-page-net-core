namespace OWP.OtherSport.Validation.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Models;
    using OfficeOpenXml;
    using OWP.OtherSport.Validation.Extensions;

    public interface ExcelReader
    {
        UploadingDataFromExcel Read(Stream stream);
    }

    public class ExcelReaderImp : ExcelReader
    {
        private const int StartRow = 2;

        private static readonly Regex otherTraderRegex = new Regex(@"#[^\s#]+", RegexOptions.IgnoreCase);

        public UploadingDataFromExcel Read(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);

            using var package = new ExcelPackage(stream);
            var sheet = package.Workbook.Worksheets.First();
            var assignments = sheet.ReadAllAssignments(StartRow).ToList();

            return new UploadingDataFromExcel
            {
                Assignments = assignments,
                Traders = GetTraders(assignments)
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

        private static List<string> GetTraderWithNote(List<AssignmentModelInExcel> assignments, string type)
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

        private static List<string> GetTradersWithType(AssignmentModelInExcel assignment, string type)
        {
            var properties = typeof(AssignmentModelInExcel).GetProperties();
            var result = new List<string>();

            foreach (var prop in properties)
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
            return matches.Cast<Match>().FirstOrDefault()?.Value.Remove(0, 1);
        }
    }
}