namespace OWP.WorkingShift.ViewLog.Services;

using Models;

public class UpdateShiftCodeTemplateBuilder : TemplateBuilder
{
    public UpdateShiftCodeTemplateBuilder(LogService logService, LogData logData)
        : base(logService, logData)
    { }

    public override string BuildOldColumn()
    {
        if (logData.UserLog.DataOld == null)
        {
            return string.Empty;
        }
        var data = logService.Parse<UpdateShiftCodeModel>(logData.UserLog.DataOld)[0];
        return BuildColumn(data);
    }

    public override string BuildNewColumn()
    {
        if (string.IsNullOrEmpty(logData.UserLog.Data))
        {
            return string.Empty;
        }
        var data = logService.Parse<UpdateShiftCodeModel>(logData.UserLog.Data)[0];
        return BuildColumn(data);
    }

    private string BuildColumn(UpdateShiftCodeModel model)
    {
        var shiftCode = GetShiftCode(model.ShiftId);
        var refShiftCode = model.IsOvertime ? $"/{GetShiftCode(model.RefShiftId)}" : string.Empty;
        return
            string.IsNullOrEmpty(shiftCode) && string.IsNullOrEmpty(refShiftCode) ?
            string.Empty : $"{model.ShiftDate:MM/dd/yyyy}:{shiftCode}{refShiftCode} <br/>";
    }
}