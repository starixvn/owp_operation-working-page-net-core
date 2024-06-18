namespace OWP.WorkingShift.ViewLog.Services;

using System;
using System.Linq;
using Constants;
using Models;
using System.Collections.Generic;

public interface TemplateService
{
    string BuildTemplate(TemplateModel model);

    string BuildLogTemplate(LogData logData);
}

public class TemplateServiceImp : TemplateService
{
    private readonly LogService logService;

    public TemplateServiceImp(LogService logService)
    {
        this.logService = logService;
    }

    public string BuildTemplate(TemplateModel model)
    {
        var logTableHeader = new List<string>
        {
           "<thead>",
            "<tr>",
            "<th width='160'>Date (GMT+8)</th>",
            "<th width='110'>Modified by</th>",
            "<th width='200'>Action</th>",
            "<th>Old Value</th>",
            "<th>New Value</th>",
            "</tr>",
            "</thead>"
        };
        var logHeader = string.Join(string.Empty, logTableHeader);
        var logContent = string.Join(string.Empty, model.UserLogs.Select(userLog => BuildLogTemplate(new LogData
        {
            UserLog = userLog,
            Shifts = model.Shifts
        })));
        var logTableBody = new List<string>
        {
            "<tbody class='log-table-body'>",
            logContent,
            "</tbody>"
        };
        var logBody = string.Join(string.Empty, logTableBody);

        return string.Join(string.Empty, logHeader, logBody);
    }

    public string BuildLogTemplate(LogData logData)
    {
        var templateBuilder = CreateBuilder(logData);
        var logRow = new List<string>
        {
             "<tr>",
            $"<td>{templateBuilder.BuildDateColumn()}</td>",
            $"<td>{templateBuilder.BuildModifiedByColumn()}</td>",
            $"<td>{templateBuilder.BuildActionColumn()}</td>",
            $"<td>{templateBuilder.BuildOldColumn()}</td>",
            $"<td>{templateBuilder.BuildNewColumn()}</td>",
            "</tr>"
        };
        return string.Join(string.Empty, logRow);
    }

#pragma warning disable S1541 // Methods and properties should not be too complex
    private TemplateBuilder CreateBuilder(LogData logData)
#pragma warning restore S1541 // Methods and properties should not be too complex
    {
        switch (logData.UserLog.ActionId)
        {
            case (int)ActionId.UpdateDefaultShift:
                return new UpdateDefaultShiftTemplateBuilder(logService, logData);

            case (int)ActionId.CopySingleShift:
                return new CopySingleShiftTemplateBuilder(logService, logData);

            case (int)ActionId.CopyWholeShifts:
                return new CopyWholeShiftsTemplateBuilder(logService, logData);

            case (int)ActionId.DeleteWholeShifts:
                return new DeleteWholeShiftsTemplateBuilder(logService, logData);

            case (int)ActionId.UpdateShiftCode:
                return new UpdateShiftCodeTemplateBuilder(logService, logData);

            case (int)ActionId.DeleteShift:
                return new DeleteShiftTemplateBuilder(logService, logData);

            case (int)ActionId.ApplyDefaultShift:
            case (int)ActionId.UploadWorkingShift:
            case (int)ActionId.CopyFromPreviousWeek:
                return new TemplateShiftBuilderBase(logService, logData);

            case (int)ActionId.PublishShift:
                return new PublishShiftTemplateBuilder(logService, logData);

            default:
                throw new NotImplementedException();
        }
    }
}