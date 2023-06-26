using OfficeOpenXml;
using OWP.OtherSport.Validation.Helpers;
using OWP.OtherSport.Validation.Models;
using System.Reflection;

namespace OWP.OtherSport.Validation.Extensions;

public static class WorkSheetExtensions
{
    public class AssignmentModelInExcelPropertyInfo
    {
        public PropertyInfo PropertyInfo { get; set; }

        public string DisplayName { get; set; }

        public List<int> ColumnIndices { get; set; } = new();

        public Type PropertyType => PropertyInfo.PropertyType;
    }

    public static Dictionary<string, int> GetHeaderMap(this ExcelWorksheet sheet)
    {
        var result = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
        var type = typeof(AssignmentModelInExcel);
        var properties = type.GetProperties()
            .Where(x => x.Name != "RowIndex").ToList();
        var headers = sheet.GetHeaderNames().ToList();

        foreach (var property in properties)
        {
            var displayName = PropertyHelper.GetDisplayName(property);
            var headerIndex = headers.FindIndex(h => h.Equals(displayName, StringComparison.CurrentCultureIgnoreCase));
            result[displayName] = headerIndex + 1;
        }

        return result;
    }

    public static IEnumerable<AssignmentModelInExcel> ReadAllAssignments(this ExcelWorksheet sheet, int row = 1)
    {
        var properties = GetAssignmentModelInExcelPropertyInfo(sheet);

        for (var i = row; i <= sheet.Dimension.End.Row; i++)
        {
            if (IsEmptyRow(sheet, i))
            {
                continue;
            }

            var assignment = ReadAssignment(sheet, properties, i);

            yield return assignment;
        }
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

    private static List<AssignmentModelInExcelPropertyInfo> GetAssignmentModelInExcelPropertyInfo(this ExcelWorksheet sheet)
    {
        var headerNames = sheet.GetHeaderNames().ToList();

        var type = typeof(AssignmentModelInExcel);
        var properties = type.GetProperties();

        return properties.Select(p =>
        {
            var name = PropertyHelper.GetDisplayName(p);

            return new AssignmentModelInExcelPropertyInfo
            {
                PropertyInfo = p,
                DisplayName = PropertyHelper.GetDisplayName(p),
                ColumnIndices = GetItemIndices(headerNames, name).ToList()
            };
        }).ToList();
    }

    private static IEnumerable<int> GetItemIndices(IEnumerable<string> items, string value)
    {
        var index = 1;
        foreach (var item in items)
        {
            if (item.Equals(value, StringComparison.CurrentCultureIgnoreCase))
            {
                yield return index;
            }

            index++;
        }
    }

    private static AssignmentModelInExcel ReadAssignment(this ExcelWorksheet sheet, List<AssignmentModelInExcelPropertyInfo> properties, int row = 1)
    {
        var methods = typeof(ExcelWorksheet).GetMethods(BindingFlags.Public | BindingFlags.Instance);
        var getValueMethod = methods.First(m => m.IsGenericMethod && m.Name == "GetValue");

        var model = new AssignmentModelInExcel
        {
            RowIndex = row
        };

        foreach (var property in properties)
        {
            if (!property.ColumnIndices.Any())
            {
                continue;
            }

            var defaultIndex = property.ColumnIndices[0];

            if (sheet.ReadSpecialProperty(model, property, row, defaultIndex))
            {
                continue;
            }

            var getValueMethodGeneric = getValueMethod.MakeGenericMethod(property.PropertyType);
            var excelValue = getValueMethodGeneric.Invoke(sheet, new object[] { row, defaultIndex });

            property.PropertyInfo.SetValue(model, excelValue);
        }

        return model;
    }

    private static bool ReadSpecialProperty(this ExcelWorksheet sheet, AssignmentModelInExcel model, AssignmentModelInExcelPropertyInfo property, int row, int column)
    {
        if (property.PropertyType != typeof(string))
        {
            return false;
        }

        var cellValue = sheet.GetValue<string>(row, column) ?? string.Empty;
        property.PropertyInfo.SetValue(model, cellValue.Trim());
        return true;

    }

    private static bool IsEmptyRow(this ExcelWorksheet sheet, int row = 1)
    {
        for (var col = 1; col <= sheet.Dimension.End.Column; col++)
        {
            if (sheet.Cells[row, col].Value != null)
            {
                return false;
            }
        }

        return true;
    }
}