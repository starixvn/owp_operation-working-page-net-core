using OfficeOpenXml;
using OWP.PreMatchReport.Constants;
using OWP.Services;

namespace OWP.PreMatchReport.Services;

public interface PreMatchReportExportService
{
    Task<byte[]> Export();
}

public class PreMatchReportExportServiceImp : PreMatchReportExportService
{
    private readonly ConfigurationService configurationService;

    public PreMatchReportExportServiceImp(ConfigurationService configurationService)
    {
        this.configurationService = configurationService;
    }

    private FileInfo GetReportTemplate(string fileName)
    {
        var templatePath = @$"App_Data/Templates/{fileName}";
        var absoluteTemplatePath = $"{configurationService.CurrentDirectory}{Path.DirectorySeparatorChar}{templatePath}";

        return new FileInfo(absoluteTemplatePath);
    }

    public async Task<byte[]> Export()
    {
        var file = GetReportTemplate(ReportConst.ReportTemplate);

        if (!file.Exists)
        {
            throw new FileNotFoundException(ReportConst.ReportTemplate);
        }

        using var package = new ExcelPackage(file);

        return await package.GetAsByteArrayAsync();
    }
}