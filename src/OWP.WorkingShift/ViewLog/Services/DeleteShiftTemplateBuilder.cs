namespace OWP.WorkingShift.ViewLog.Services;

using System.Linq;
using Models;

public class DeleteShiftTemplateBuilder : TemplateBuilder
{
    public DeleteShiftTemplateBuilder(LogService logService, LogData logData)
        : base(logService, logData)
    { }

    public override string BuildOldColumn()
    {
        return BuildTemplate(logData.UserLog.DataOld);
    }

    public override string BuildNewColumn()
    {
        return BuildTemplate(logData.UserLog.Data);
    }

    private string BuildTemplate(string data)
    {
        if (string.IsNullOrWhiteSpace(data))
        {
            return string.Empty;
        }

        var shiftModel = logService.Parse<DeleteShiftModel>(data).First();
        var shiftCodeFormat = !string.IsNullOrWhiteSpace(shiftModel.RefShiftId) ?
            $"{GetShiftCode(shiftModel.ShiftId)}/{GetShiftCode(shiftModel.RefShiftId)}" :
            $"{GetShiftCode(shiftModel.ShiftId)}";
        return string.IsNullOrWhiteSpace(shiftCodeFormat) ?
            string.Empty :
            $"{shiftModel.Date.ToString("MM/dd/yyyy")}: {shiftCodeFormat}";
    }
}