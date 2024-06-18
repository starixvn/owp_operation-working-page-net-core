namespace OWP.WorkingShift.WeeklyWorkingShift.Models;

using System;
using Enumerations;

public class ExportModel : QueryModel
{
    public ExportType ExportType { get; set; }

    public string GetExportText()
    {
        switch (ExportType)
        {
            case ExportType.SMS_Local:
                return "[SMS, WAH HCMC] Local";

            case ExportType.SMS_Local_LA_LST:
                return "[SMS, WAH HCMC] Local LA, LST";

            case ExportType.SMS_Expat:
                return "[SMS, WAH HCMC] Expat";

            default:
                return string.Empty;
        }
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