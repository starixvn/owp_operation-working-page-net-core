namespace OWP.WorkingShift.ViewLog.Services;

using System;
using Models;

public class TemplateBuilder
{
    protected readonly LogService logService;
    protected readonly LogData logData;

    protected TemplateBuilder(LogService logService, LogData logData)
    {
        this.logService = logService;
        this.logData = logData;
    }

    public virtual string BuildDateColumn()
    {
        return logData.UserLog.ModifiedDate.ToGmt8().ToString("MM/dd/yyyy hh:mm tt");
    }

    public virtual string BuildModifiedByColumn()
    {
        return logData.UserLog.ModifiedBy ?? string.Empty;
    }

    public virtual string BuildActionColumn()
    {
        return logService.GetActionName(logData.UserLog.ActionId);
    }

    public virtual string BuildOldColumn()
    {
        return string.Empty;
    }

    public virtual string BuildNewColumn()
    {
        return string.Empty;
    }

    protected string GetShiftCode(int shiftId)
    {
        return logData.Shifts.ContainsKey(shiftId) ? logData.Shifts[shiftId] : "The shift has been removed";
    }

    protected string GetShiftCode(string shiftId)
    {
        return string.IsNullOrWhiteSpace(shiftId) ? string.Empty : GetShiftCode(Convert.ToInt32(shiftId));
    }
}