namespace OWP.WorkingShift.ViewLog.Services;

using System.Collections.Generic;
using Models;

public class UploadWorkingShiftTemplateBuilder : TemplateBuilder
{
    public UploadWorkingShiftTemplateBuilder(LogService logService, LogData logData)
        : base(logService, logData)
    { }

    public override string BuildNewColumn()
    {
        if (string.IsNullOrEmpty(logData.UserLog.Data))
        {
            return string.Empty;
        }
        var uploadWorkingShiftModel = logService.Parse<UploadWorkingShiftModel>(logData.UserLog.Data);
        var content = new List<string>();
        foreach (var model in uploadWorkingShiftModel)
        {
            content.Add($"{model.Date:MM/dd/yyyy}:{logData.Shifts[model.ShiftId]}");
        }
        return string.Join(",", content);
    }
}