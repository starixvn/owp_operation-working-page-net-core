namespace OWP.Event.LiveGameAssignment.Download.Extensions;

using Models;
using NPOI.SS.UserModel;
using OWP.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

public static class ExcelIO
{
    public static void WriteExcel<T>(this IEnumerable<T> source, TemplateWriter writer, Stream target, Type exportType)
    {
        var rowWriter = GetWriter<T>(writer);

        var sheet = writer.CustomizedSheet;
        var index = 0;

        foreach (var e in source)
        {
            var row = sheet.CreateRow(writer.StartRow + index);

            rowWriter(row, e);
            index++;
        }

        writer.WrapUp(exportType);

        sheet.Workbook.Write(target);
    }

    #region general util

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Reviewed.")]
    private static Dictionary<string, int> GetColumns(ISheet sheet)
    {
        var row = sheet.GetRow(0);
        return row.Cells.ToDictionary(cell => cell.StringCellValue, cell => cell.ColumnIndex);
    }

    #endregion general util

    #region writer helper

    private static Action<IRow, T> GetWriter<T>(TemplateWriter writer)
    {
        var param1 = Expression.Parameter(typeof(IRow));
        var param2 = Expression.Parameter(typeof(T));
        var body = WriteObj(writer, param1, param2, typeof(T));
        return Expression.Lambda<Action<IRow, T>>(body, param1, param2).Compile();
    }

    private static Expression WriteObj(TemplateWriter writer, Expression row, Expression obj, Type type)
    {
        return
            type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.GetCustomAttribute<ExcelIgnoreAttribute>() == null)
                .Select(p => WriteProp(writer, row, obj, p))
                .Aggregate(Expression.Block);
    }

    public static bool IsBasicType(Type t)
    {
        var temp = t;

        if (t != null && t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            temp = t.GetGenericArguments()[0];
        }

        return temp == typeof(int) || temp == typeof(double) || temp == typeof(string) || temp == typeof(DateTime);
    }

    private static Expression WriteProp(TemplateWriter writer, Expression row, Expression obj, PropertyInfo prop)
    {
        var value = Expression.Property(obj, prop);
        Expression result = IsBasicType(prop.PropertyType)
            ? Expression.Call(
                Expression.Constant(writer),
                typeof(TemplateWriter).GetMethod("WriteColumns"),
                row,
                Expression.Constant(prop),
                Expression.Convert(value, typeof(object)))
            : WriteObj(writer, row, value, prop.PropertyType);
        return result;
    }

    #endregion writer helper
    #region reader helper

    private static Expression GetCreateExp(TemplateReader reader, Expression row, Expression lambda, Type type)
    {
        var obj = Expression.Variable(type);

        Expression assignExpression = Expression.Assign(obj, Expression.New(type));
        var concat =
            type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.GetCustomAttribute<ExcelIgnoreAttribute>() == null)
                .Select(p => MapProp(reader, row, lambda, obj, p))
                .Aggregate(assignExpression, Expression.Block);

        return Expression.Block(new[] { obj }, concat, obj); // NOSONAR
    }

    private static Expression MapProp(TemplateReader reader, Expression row, Expression lambda, Expression obj, PropertyInfo prop)
    {
        var left = Expression.Property(obj, prop);
        Expression right;

        if (reader.BasicType(prop.PropertyType))
        {
            var name = reader.GetColumnAttributeName(prop);
            var idxExpr = Expression.Constant(name);
            right = Expression.Convert(Expression.Invoke(lambda, idxExpr, row, Expression.Constant(prop.PropertyType)), prop.PropertyType);
        }
        else
        {
            right = GetCreateExp(reader, row, lambda, prop.PropertyType);
        }

        return Expression.Assign(left, right);
    }

    #endregion reader helper
}