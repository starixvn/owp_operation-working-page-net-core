namespace OWP.Extensions;

using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Globalization;

public static class XSSFCellExtensions
{
    public static void SetCellValue(this ICell cell, object value)
    {
        if (value == null)
        {
            cell.SetCellValue(string.Empty);
        }
        else
        {
            var typeValue = value.GetType();

            if (typeValue == typeof(string))
            {
                cell.SetCellValue((string)value);
            }
            else if (typeValue == typeof(int))
            {
                cell.SetCellValue((int)value);
            }
            else if (typeValue == typeof(long))
            {
                cell.SetCellValue((long)value);
            }
            else if (typeValue == typeof(double))
            {
                cell.SetCellValue((double)value);
            }
            else if (typeValue == typeof(decimal))
            {
                cell.SetCellValue((decimal)value);
            }
            else if (typeValue == typeof(bool))
            {
                cell.SetCellValue((bool)value);
            }
            else if (typeValue == typeof(DateTime))
            {
                cell.SetCellValue((DateTime)value);
            }
            else
            {
                cell.SetCellValue(value.ToString());
            }
        }
    }

    public static void SetCellValue(this ICell cell, double? value)
    {
        var currentValue = value ?? 0;
        cell.SetCellValue(currentValue);
    }

    public static void SetCellValue(this ICell cell, decimal value)
    {
        cell.SetCellValue(Convert.ToDouble(value, CultureInfo.InvariantCulture));
    }

    public static void SetCellValue(this ICell cell, decimal? value)
    {
        var currentValue = value ?? 0;
        cell.SetCellValue(currentValue);
    }

    public static void SetCellValue(this ICell cell, int value)
    {
        cell.SetCellValue(Convert.ToDouble(value, CultureInfo.InvariantCulture));
    }

    public static void SetCellValue(this ICell cell, int? value)
    {
        var currentValue = value ?? 0;
        cell.SetCellValue(currentValue);
    }

    public static void SetCellValue(this ICell cell, long value)
    {
        cell.SetCellValue(Convert.ToDouble(value, CultureInfo.InvariantCulture));
    }

    public static void SetCellValue(this ICell cell, long? value)
    {
        var currentValue = value ?? 0;
        cell.SetCellValue(currentValue);
    }

    public static void CopyFrom(this ICell cell, ICell sourceCell)
    {
        // Copy merged cells.
        CellRangeAddress sourceRange = null;

        if (cell != null && sourceCell != null)
        {
            var numMergedRegions = sourceCell.Sheet.NumMergedRegions;
            var i = 0;
            var isContinue = true;
            while (isContinue && i < numMergedRegions)
            {
                var range = sourceCell.Sheet.GetMergedRegion(i);

                if (range.FirstColumn <= sourceCell.ColumnIndex
                    && range.LastColumn >= sourceCell.ColumnIndex
                    && range.FirstRow <= sourceCell.RowIndex
                    && range.LastRow >= sourceCell.RowIndex)
                {
                    sourceRange = range;
                    isContinue = false;
                }

                i++;
            }

            if (sourceRange != null)
            {
                cell.Sheet.AddMergedRegion(Clone(sourceRange));
            }

            // Copy style
            cell.CellStyle = sourceCell.CellStyle;
        }
    }

    public static CellRangeAddress Clone(CellRangeAddressBase range)
    {
        var result = new CellRangeAddress(range.FirstRow, range.LastRow, range.FirstColumn, range.LastColumn);

        return result;
    }
}