namespace OWP.WorkingShift.WeeklyWorkingShift.Models;

using Enumerations;
using System;

public class ExportModel : QueryModel
{
    public ExportType ExportType { get; set; }

    public string GetExportText()
    {
        return ExportType switch
        {
            ExportType.SMS_Local => "[SMS, WAH HCMC] Local",
            ExportType.SMS_Local_LA_LST => "[SMS, WAH HCMC] Local LA, LST",
            ExportType.SMS_Expat => "[SMS, WAH HCMC] Expat",
            _ => string.Empty
        };
    }

    public string GetExportFileName()
    {
        var tagName = "OPS";
        if (ExportType != ExportType.Filter)
        {
            tagName = GetExportText();
        }

        return $"{tagName} Schedule for Driver {ConvertDateTimeToString(FromDate)} to {ConvertDateTimeToString(ToDate)}.xlsx";
    }

    public string GetSheetName() => $"{ConvertDateTimeToString(FromDate)}-{ConvertDateTimeToString(ToDate)}";

    private static string ConvertDateTimeToString(DateTime date) => date.ToString("dd-MMM-yyyy");
}