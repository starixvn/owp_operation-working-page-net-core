namespace OWP.WorkingShift.ViewLog.Services;

using System;
using System.Linq;
using Models;
using OWP.WorkingShift.ViewLog.Models;

public class PublishShiftTemplateBuilder : TemplateShiftBuilderBase
{
    private const string Template = "Publish working shift from {0} to {1}: {2}";

    public PublishShiftTemplateBuilder(LogService logService, LogData logData)
        : base(logService, logData)
    { }

    public override string BuildNewColumn()
    {
        return BuildTemplate(logData.UserLog.Data);
    }

    public override string BuildOldColumn()
    {
        return BuildTemplate(logData.UserLog.DataOld);
    }

    public string BuildTemplate(string logdata)
    {
        if (string.IsNullOrWhiteSpace(logdata))
        {
            return string.Empty;
        }
        var publicShiftModel = logService.Parse<PublicShiftModel>(logdata).First();
        var args = new object[]
        {
            publicShiftModel.FromDate.ToString("MM/dd/yyyy"),
            publicShiftModel.ToDate.ToString("MM/dd/yyyy"),
            publicShiftModel.Isp?"Yes":"No"
        };
        return publicShiftModel.FromDate == DateTime.MinValue ?
            string.Empty :
            string.Format(Template, args);
    }
}