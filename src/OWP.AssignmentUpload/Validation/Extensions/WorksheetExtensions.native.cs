namespace OWP.AssignmentUpload.Validation.Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Anotations;
using Constants;
using Exceptions;
using Models;
using OfficeOpenXml;

public static class WorksheetExtensions
{
    public class AssignmentModelInExcelPropertyInfo
    {
        public PropertyInfo PropertyInfo { get; set; }

        public string DisplayName { get; set; }

        public bool IsMaxBet { get; set; }

        public bool IsSpread { get; set; }

        public List<int> ColumnIndices { get; set; } = new List<int>();

        public Type PropertyType => PropertyInfo.PropertyType;
    }

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

    public static DateTime GetDateTime(this ExcelWorksheet sheet)
    {
        var cellValue = sheet.GetValue(1, 2);

        if (cellValue == null || string.IsNullOrEmpty(cellValue.ToString()))
        {
            throw new InvalidDateTimeException();
        }

        if (cellValue is double)
        {
            if ((double)cellValue < EncodeDate.MIN_VALUE || (double)cellValue > EncodeDate.MAX_VALUE)
            {
                throw new InvalidDateTimeException();
            }
            return DateTime.FromOADate((double)cellValue);
        }

        if (cellValue is string)
        {
            DateTime value;
            if (!DateTime.TryParse(cellValue.ToString(), out value))
            {
                throw new InvalidDateTimeException();
            }
            return value;
        }

        if (cellValue is DateTime)
        {
            return sheet.GetValue<DateTime>(1, 2);
        }

        throw new InvalidDateTimeException();
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

            if (!IsEmptyAssignment(assignment))
            {
                yield return assignment;
            }
        }
    }

    private static List<AssignmentModelInExcelPropertyInfo> GetAssignmentModelInExcelPropertyInfo(this ExcelWorksheet sheet)
    {
        var headerNames = sheet.GetHeaderNames().ToList();

        var type = typeof(AssignmentModelInExcel);
        var properties = type.GetProperties();

        return properties.Select(p =>
        {
            var name = GetDisplayName(p);

            return new AssignmentModelInExcelPropertyInfo
            {
                PropertyInfo = p,
                DisplayName = GetDisplayName(p),
                IsMaxBet = p.GetCustomAttributes(typeof(MaxBetAttribute), true).Any(),
                IsSpread = p.GetCustomAttributes(typeof(SpreadAttribute), true).Any(),
                ColumnIndices = GetItemIndices(headerNames, name).ToList()
            };
        }).ToList();
    }

    private static IEnumerable<string> GetHeaderNames(this ExcelWorksheet sheet)
    {
        for (var i = 1; i <= sheet.Dimension.End.Column; i++)
        {
            var cellValue = sheet.GetValue<string>(2, i);
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

        var model = new AssignmentModelInExcel();

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
        if (property.DisplayName.Equals("Note", StringComparison.CurrentCultureIgnoreCase))
        {
            var notes = GetNotes(sheet, row, property.ColumnIndices);
            property.PropertyInfo.SetValue(model, notes);
            return true;
        }

        if (property.PropertyType == typeof(int))
        {
            var cellValue = sheet.GetValue<string>(row, column);
            int value;
            int.TryParse(cellValue, out value);
            property.PropertyInfo.SetValue(model, value);
            return true;
        }

        if (property.PropertyType == typeof(string))
        {
            var cellValue = sheet.GetValue<string>(row, column) ?? string.Empty;
            property.PropertyInfo.SetValue(model, cellValue.Trim());
            return true;
        }

        if (property.PropertyType == typeof(decimal?) && property.IsMaxBet)
        {
            var cellValue = sheet.GetValue<string>(row, column);
            var value = GetMaxbetValue(cellValue);
            property.PropertyInfo.SetValue(model, value);
            return true;
        }

        if (property.PropertyType == typeof(decimal?) && property.IsSpread)
        {
            var cellValue = sheet.GetValue<string>(row, column);
            var value = GetSpreadValue(cellValue);
            property.PropertyInfo.SetValue(model, value);
            return true;
        }

        return false;
    }

    private static List<string> GetNotes(this ExcelWorksheet sheet, int row, List<int> columnIndices)
    {
        return columnIndices.Select(i =>
        {
            var cellValue = sheet.GetValue<string>(row, i) ?? string.Empty;
            return cellValue.Trim();
        }).ToList();
    }

    public static decimal? GetMaxbetValue(string maxbet)
    {
        const NumberStyles style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;
        const int K_VALUE = 1000;

        maxbet = maxbet ?? string.Empty;
        decimal tempValue;
        bool valid;

        if (maxbet.Contains("K") || maxbet.Contains("k"))
        {
            maxbet = maxbet.Replace("K", "").Replace("k", "");
            valid = decimal.TryParse(maxbet, style, CultureInfo.InvariantCulture, out tempValue);
            tempValue = tempValue * K_VALUE;
        }
        else
        {
            valid = decimal.TryParse(maxbet, style, CultureInfo.InvariantCulture, out tempValue);
        }

        return valid ? new decimal?(tempValue) : null;
    }

    private static decimal? GetSpreadValue(string spread)
    {
        const NumberStyles style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;
        const decimal SPREAD_MULTIPLIER = 0.01m;

        decimal tempValue;
        var valid = decimal.TryParse(spread, style, CultureInfo.InvariantCulture, out tempValue);

        return valid ? new decimal?(tempValue * SPREAD_MULTIPLIER) : null;
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

    private static bool IsEmptyAssignment(AssignmentModelInExcel assignment)
    {
        return string.IsNullOrEmpty(assignment.League)
               || string.IsNullOrEmpty(assignment.Home)
               || string.IsNullOrEmpty(assignment.Away);
    }
}