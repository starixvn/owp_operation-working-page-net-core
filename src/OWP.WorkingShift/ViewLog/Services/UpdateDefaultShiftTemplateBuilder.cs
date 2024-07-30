namespace OWP.WorkingShift.ViewLog.Services;

using Models;
using System.Collections.Generic;
using System.Linq;

public class UpdateDefaultShiftTemplateBuilder : TemplateBuilder
{
    public UpdateDefaultShiftTemplateBuilder(LogService logService, LogData logData)
        : base(logService, logData)
    { }

    public override string BuildOldColumn()
    {
        if (logData.UserLog.DataOld == null)
        {
            return string.Empty;
        }

        var model = logService.Parse<UpdateDefaultShiftModel>(logData.UserLog.DataOld)[0];
        return BuildColumn(model);
    }

    public override string BuildNewColumn()
    {
        if (string.IsNullOrEmpty(logData.UserLog.Data))
        {
            return string.Empty;
        }
        var model = logService.Parse<UpdateDefaultShiftModel>(logData.UserLog.Data)[0];
        return BuildColumn(model);
    }

    private string BuildColumn(UpdateDefaultShiftModel model)
    {
        const string formatDateTime = "MM/dd/yyyy";
        var shiftRows = string.Join(string.Empty, model.ShiftIds.Split(',').Select((shiftId, index) =>
        {
            if (string.IsNullOrWhiteSpace(shiftId))
            {
                return string.Empty;
            }

            var shiftDate = model.FromDate.AddDays(index);
            var shiftCode = GetShiftCode(shiftId);
            return $"<p>{shiftDate.ToString(formatDateTime)}: {shiftCode}</p>";
        }));


        var fromDate = model.FromDate.ToString(formatDateTime);
        var toDate = model.ToDate.ToString(formatDateTime);
        var shiftRowHtml = new List<string>
        {
             $"<p>Apply for the current week ({fromDate} to {toDate}): " + (model.IsApplyCurrentWeek ? "Yes" : "No") + "</p>",
            "<br/>",
            shiftRows
        };
        return string.Join(string.Empty, shiftRowHtml);
    }
}