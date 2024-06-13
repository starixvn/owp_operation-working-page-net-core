namespace OWP.WorkingShift.ViewLog.Services;

using System.Linq;
using Models;

public class CopySingleShiftTemplateBuilder : TemplateBuilder
{
    public CopySingleShiftTemplateBuilder(LogService logService, LogData logData)
        : base(logService, logData)
    { }

    public override string BuildNewColumn()
    {
        if (string.IsNullOrEmpty(logData.UserLog.Data))
        {
            return string.Empty;
        }
        var data = logService.Parse<CopySingleShiftModel>(logData.UserLog.Data).First();
        return BuildColumn(data);
    }

    public override string BuildOldColumn()
    {
        if (string.IsNullOrEmpty(logData.UserLog.DataOld))
        {
            return string.Empty;
        }
        var data = logService.Parse<CopySingleShiftModel>(logData.UserLog.DataOld).First();
        return BuildColumn(data);
    }

    private string BuildColumn(CopySingleShiftModel data)
    {
        var shiftCode = GetShiftCode(data.ShiftId);
        var refShift = !string.IsNullOrEmpty(data.RefShiftId) ? $"/{GetShiftCode(data.RefShiftId)}" : string.Empty;
        return string.IsNullOrEmpty(shiftCode) && string.IsNullOrEmpty(refShift) ? string.Empty : $"{data.ShiftDate.ToString("MM/dd/yyyy")}: {shiftCode}{refShift}";
    }
}