namespace OWP.OtherSport.Excel.Extensions
{
    using System;
    using System.Collections.Generic;
    using OfficeOpenXml;
    using OWP.OtherSport.Excel.Models;

    public static class WorkSheetExtensions
    {
        public static Dictionary<string, int> GetHeaderMap(this ExcelWorksheet sheet)
        {
            var result = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
            var type = typeof(AssignmentModelInExcel);
            var properties = type.GetProperties();
            var propertiesWithoutNotes = properties.Where(x => x.Name != "Notes").ToList();

            var headers = sheet.GetHeaderNames().ToList();
            foreach (var property in propertiesWithoutNotes)
            {
                var displayName = GetDisplayName(property);
                var headerIndex = headers.FindIndex(h => h.Equals(displayName, StringComparison.CurrentCultureIgnoreCase));
                result[displayName] = headerIndex + 1;
            }

            return result;
        }
    }
}