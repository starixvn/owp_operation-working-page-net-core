using Epplus.Mapper.Extensions;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OWP.EventPicking.Helper;
using OWP.EventPicking.Models;
using OWP.Services;

namespace OWP.EventPicking.Services;

public interface ExportService
{
    Task<FileStreamResult> BuildFileStream(IList<EventPickingModel> matches);
}

public class ExportServiceImp : ExportService, IDisposable
{
    private ExcelPackage excelPackage;

    private readonly ConfigurationService configurationService;

    public ExportServiceImp(ConfigurationService configurationService)
    {
        this.configurationService = configurationService;
    }

    public async Task<FileStreamResult> BuildFileStream(IList<EventPickingModel> matches)
    {
        var templatePath = Utils.GetMiscMatchTemplate(configurationService.CurrentDirectory);

        await using var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read);

        excelPackage = new ExcelPackage(stream);
        var templateSheet = excelPackage.Workbook.Worksheets.First();
        templateSheet.ApplyVertical(matches, 2);
        ExcelColorHelper.FillRowColor(matches, templateSheet);
        ExcelColorHelper.FillCellColor(matches, templateSheet);

        var fileStream = new MemoryStream();
        await excelPackage.SaveAsAsync(fileStream);
        fileStream.Position = 0;
        const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        return new FileStreamResult(fileStream, contentType);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }


    protected virtual void Dispose(bool isDisposing)
    {
        excelPackage?.Dispose();
    }
}