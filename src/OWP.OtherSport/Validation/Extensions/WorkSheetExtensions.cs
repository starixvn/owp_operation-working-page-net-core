namespace OWP.OtherSport.Validation.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using OfficeOpenXml;
    using OWP.OtherSport.Validation.Models;

    public static class WorkSheetExtensions
    {
        public static Dictionary<string, int> GetHeaderMap(this ExcelWorksheet sheet)
        {
            var result = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
            var type = typeof(AssignmentModelInExcel);
            var properties = type.GetProperties().ToList();
            var headers = sheet.GetHeaderNames().ToList();

            foreach (var property in properties)
            {
                var displayName = GetDisplayName(property);
                var headerIndex = headers.FindIndex(h => h.Equals(displayName, StringComparison.CurrentCultureIgnoreCase));
                result[displayName] = headerIndex + 1;
            }

            return result;
        }

        private static IEnumerable<string> GetHeaderNames(this ExcelWorksheet sheet)
        {
            for (var i = 1; i <= sheet.Dimension.End.Column; i++)
            {
                var cellValue = sheet.GetValue<string>(1, i);
                if (string.IsNullOrWhiteSpace(cellValue))
                {
                    continue;
                }

                cellValue = cellValue.Trim();

                yield return cellValue;
            }
        }

        private static string GetDisplayName(PropertyInfo property)
        {
            var attribute = property.GetCustomAttributes(typeof(DisplayAttribute), true)
                  .Cast<DisplayAttribute>()
                  .FirstOrDefault();
            return attribute == null
                ? string.Empty
                : attribute.Name;
        }
    }
}