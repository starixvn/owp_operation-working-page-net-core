namespace OWP.Event.LiveGameAssignment.Download.Models;

using Extensions;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OWP.Event.EventShared.Enum;
using OWP.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Utils;

public abstract class TemplateWriter // NOSONAR
{
    public abstract ISheet CustomizedSheet { get; }

    public abstract int StartRow { get; }

    public abstract int HeaderRow { get; }

    public abstract void WriteColumns(IRow row, PropertyInfo prop, object value);

    public abstract void WrapUp(Type exportType);
}

public class AssignmentWriter : TemplateWriter
{
    private readonly string[] originalMatchStatus = { "Live", "In-play", "Completed", "Cancel", "Postponed", "Suspended" };
    private readonly string[] highlightMatchStatus = { "Cancel", "Postponed", "Suspended" };
    private readonly int _maxNote;
    private readonly int maxLstNote;
    private readonly ISheet _sheet;
    private readonly DownLoadExportModel _model;
    private readonly ICellStyle highlightCellStyle;
    private readonly ICellStyle matchStatusCellStyle;
    private const int startRow = 2;
    private const int headerRow = 1;
    private readonly List<ColumnName> colorColumns = new() { ColumnName.Time, ColumnName.League, ColumnName.Home, ColumnName.Away };
    private readonly List<ColumnName> colorMatchStatus = new() { ColumnName.MatchStatus };

    public AssignmentWriter(int maxNote, DownLoadExportModel model)
    {
        _maxNote = maxNote;
        _model = model;
        maxLstNote = model.PermissionModel.FullTimeView.IsRead ? 0 : 1;
        _sheet = new XSSFWorkbook().CreateSheet();
        highlightCellStyle = _sheet.Workbook.CreateCellStyle();
        highlightCellStyle.FillForegroundColor = HSSFColor.Grey25Percent.Index;
        highlightCellStyle.FillPattern = FillPattern.SolidForeground;

        matchStatusCellStyle = _sheet.Workbook.CreateCellStyle();
        matchStatusCellStyle.FillForegroundColor = HSSFColor.Yellow.Index;
        matchStatusCellStyle.FillPattern = FillPattern.SolidForeground;
    }

    public override ISheet CustomizedSheet => _sheet;

    public override int StartRow => startRow;

    public override int HeaderRow => headerRow;

    public override void WriteColumns(IRow row, PropertyInfo prop, object value)
    {
        var columnAttribute = prop.GetCustomAttribute<ColumnAttribute>();
        var columnIndex = (int)columnAttribute.Name;
        const int noteColumnIndex = (int)ColumnName.Note;
        const int lstnoteColumnIndex = (int)ColumnName.LSTNote;
        if (columnIndex == noteColumnIndex)
        {
            var notes = (value as IEnumerable<string>).ToList();
            for (var i = 0; i < _maxNote; i++)
            {
                columnIndex = noteColumnIndex + i - maxLstNote;
                var cell = row.CreateCell(columnIndex);
                if (i < notes.Count)
                {
                    cell.SetCellValue(notes[i]);
                    cell.CellStyle = highlightCellStyle;
                }
            }
        }
        else
        {
            if (columnIndex > lstnoteColumnIndex)
            {
                columnIndex = columnIndex + _maxNote - 1 - maxLstNote;
            }
            var cell = row.CreateCell(columnIndex);
            cell.SetCellValue(value);
            if (colorColumns.Contains(columnAttribute.Name))
            {
                cell.CellStyle = highlightCellStyle;
            }
            if (colorMatchStatus.Contains(columnAttribute.Name) && highlightMatchStatus.Contains(value))
            {
                cell.CellStyle = matchStatusCellStyle;
            }
        }
    }

    public override void WrapUp(Type exportType)
    {
        var header = CustomizedSheet.CreateRow(HeaderRow);

        var dateCell = CustomizedSheet.CreateRow(0).CreateCell(0);
        dateCell.CellStyle = CustomizedSheet.Workbook.CreateCellStyle();
        dateCell.SetCellValue("Date");
        CustomizedSheet.GetRow(0).CreateCell(1).SetCellValue(_model.EventDate.ToOWPDateFormat());
        dateCell.CellStyle.FillForegroundColor = HSSFColor.LightCornflowerBlue.Index;
        dateCell.CellStyle.FillPattern = FillPattern.SolidForeground;

        var employeeCell = CustomizedSheet.GetRow(0).CreateCell(3);
        employeeCell.CellStyle = CustomizedSheet.Workbook.CreateCellStyle();
        employeeCell.SetCellValue("Employee");
        CustomizedSheet.GetRow(0).CreateCell(4).SetCellValue(_model.Username);
        employeeCell.CellStyle.FillForegroundColor = HSSFColor.LightCornflowerBlue.Index;
        employeeCell.CellStyle.FillPattern = FillPattern.SolidForeground;

        var matchTypeCell = CustomizedSheet.GetRow(0).CreateCell(6);
        matchTypeCell.CellStyle = CustomizedSheet.Workbook.CreateCellStyle();
        matchTypeCell.SetCellValue("MatchType");
        CustomizedSheet.GetRow(0).CreateCell(7).SetCellValue(_model.MatchType);
        matchTypeCell.CellStyle.FillForegroundColor = HSSFColor.LightCornflowerBlue.Index;
        matchTypeCell.CellStyle.FillPattern = FillPattern.SolidForeground;


        var matchStatusCell = CustomizedSheet.GetRow(0).CreateCell(9);
        matchStatusCell.CellStyle = CustomizedSheet.Workbook.CreateCellStyle();
        matchStatusCell.SetCellValue("MatchStatus");
        CustomizedSheet.GetRow(0).CreateCell(10).SetCellValue(ConvertToString(_model.MatchStatusIds));
        matchStatusCell.CellStyle.FillForegroundColor = HSSFColor.LightCornflowerBlue.Index;
        matchStatusCell.CellStyle.FillPattern = FillPattern.SolidForeground;

        var syncLiveCell = CustomizedSheet.GetRow(0).CreateCell(12);
        syncLiveCell.CellStyle = CustomizedSheet.Workbook.CreateCellStyle();
        syncLiveCell.SetCellValue("SyncLive");
        CustomizedSheet.GetRow(0).CreateCell(13).SetCellValue((SyncLiveFilter)_model.SyncLiveStatus);
        syncLiveCell.CellStyle.FillForegroundColor = HSSFColor.LightCornflowerBlue.Index;
        syncLiveCell.CellStyle.FillPattern = FillPattern.SolidForeground;

        var lastUpdatedCell = CustomizedSheet.GetRow(0).CreateCell(15);
        lastUpdatedCell.CellStyle = CustomizedSheet.Workbook.CreateCellStyle();
        lastUpdatedCell.SetCellValue("LastUpdated");
        CustomizedSheet.GetRow(0).CreateCell(16).SetCellValue(_model.UploadDateTime.ToLastUpdatedFormat());
        lastUpdatedCell.CellStyle.FillForegroundColor = HSSFColor.LightCornflowerBlue.Index;
        lastUpdatedCell.CellStyle.FillPattern = FillPattern.SolidForeground;

        IterHeader(header, exportType);
    }

    private string ConvertToString(string matchStatusIds)
    {
        StringBuilder matchStatus = new("");
        foreach (var id in matchStatusIds)
        {
            if (id == ',')
            {
                matchStatus.Append(",");
            }
            else
            {
                var index = int.Parse(id.ToString());
                matchStatus.Append(originalMatchStatus[index - 1]);
            }
        }
        return matchStatus.ToString();
    }

    private void IterHeader(IRow headeRow, Type t)
    {
        t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.GetCustomAttribute<ExcelIgnoreAttribute>() == null)
            .Iter(p => IterProperty(headeRow, p));
    }

    private void IterProperty(IRow row, PropertyInfo prop)
    {
        if (ConvertTypeService.IsBasicType(prop.PropertyType))
        {
            WriteCell(row, prop);
        }
        else
        {
            IterHeader(row, prop.PropertyType);
        }
    }

    private void WriteCell(IRow row, PropertyInfo prop)
    {
        var columnAttribute = prop.GetCustomAttribute<ColumnAttribute>();
        var columnName = EnumUtils<ColumnName>.GetDescription(columnAttribute.Name);
        var columnIndex = (int)columnAttribute.Name;
        const int noteColumnIndex = (int)ColumnName.Note;
        const int lstnoteColumnIndex = (int)ColumnName.LSTNote;
        if (columnIndex == noteColumnIndex)
        {
            for (int i = 0; i < _maxNote; i++)
            {
                columnIndex = noteColumnIndex + i - maxLstNote;
                var cell = row.CreateCell(columnIndex);
                cell.SetCellValue(columnName);
                cell.CellStyle = MakeCellStyle(columnAttribute.Name);
            }
        }
        else
        {
            if (columnIndex > lstnoteColumnIndex)
            {
                columnIndex = columnIndex + _maxNote - 1 - maxLstNote;
            }
            var cell = row.CreateCell(columnIndex);
            cell.SetCellValue(columnName);
            cell.CellStyle = MakeCellStyle(columnAttribute.Name);
        }
    }

    private ICellStyle MakeCellStyle(ColumnName name)
    {
        var backgroundColor = ColumnStyle.GetBackgroundColor(name);
        var fontColor = ColumnStyle.GetFontColorColor(name);

        var font = _sheet.Workbook.CreateFont();
        font.Color = fontColor.Index;
        font.FontHeightInPoints = 11;
        var cellStyle = (XSSFCellStyle)_sheet.Workbook.CreateCellStyle();

        var rgb = new[] { backgroundColor.R, backgroundColor.G, backgroundColor.B };
        cellStyle.SetFillForegroundColor(new XSSFColor(rgb));
        cellStyle.FillPattern = FillPattern.SolidForeground;
        cellStyle.SetFont(font);

        return cellStyle;
    }
}
