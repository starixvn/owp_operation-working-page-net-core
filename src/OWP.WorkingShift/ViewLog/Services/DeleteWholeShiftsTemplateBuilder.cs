namespace OWP.WorkingShift.ViewLog.Services;

using System.Text;
using Models;

public class DeleteWholeShiftsTemplateBuilder : TemplateBuilder
{
    public DeleteWholeShiftsTemplateBuilder(LogService logService, LogData logData)
        : base(logService, logData)
    { }

    public override string BuildOldColumn()
    {
        if (string.IsNullOrEmpty(logData.UserLog.DataOld))
        {
            return string.Empty;
        }
        var data = logService.Parse<DeleteWholeShiftsModel>(logData.UserLog.DataOld);
        var html = new StringBuilder();
        foreach (var row in data)
        {
            var shiftCode = GetShiftCode(row.ShiftId);
            var refShift = !string.IsNullOrEmpty(row.RefShiftId) ? $"/{GetShiftCode(row.RefShiftId)}" : string.Empty;
            var logRow = string.IsNullOrEmpty(shiftCode) && string.IsNullOrEmpty(refShift) ? string.Empty : $"{row.ShiftDate.ToString("MM/dd/yyyy")}: {shiftCode}{refShift}<br/>";
            html.Append(logRow);
        }
        return html.ToString();
    }
}