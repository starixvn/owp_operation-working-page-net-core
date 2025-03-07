using Epplus.Mapper.Extensions;
using OWP.Constants;
using OWP.EventPicking.Api.Constant;
using OWP.EventPicking.Api.Helper;
using OWP.EventPicking.Api.Models;

namespace OWP.EventPicking.Api.Services;

public interface ExportService
{
    Task<FileStreamResult> BuildFileStream(IList<EventPickingModel> matches, bool isMiscMatch);
}

public class ExportServiceImp : ExportService, IDisposable
{
    private ExcelPackage excelPackage;

    private readonly ConfigurationService configurationService;

    private readonly PermissionService permissionService;

    public ExportServiceImp(ConfigurationService configurationService, PermissionService permissionService)
    {
        this.configurationService = configurationService;
        this.permissionService = permissionService;
    }

    public async Task<FileStreamResult> BuildFileStream(IList<EventPickingModel> matches, bool isMiscMatch)
    {
        var templatePath = Utils.GetMiscMatchTemplate(configurationService.CurrentDirectory);

        await using var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read);

        excelPackage = new ExcelPackage(stream);
        var templateSheet = excelPackage.Workbook.Worksheets.First();
        templateSheet.ApplyVertical(matches, 2);
        ExcelColorHelper.FillRowColor(matches, templateSheet);
        ExcelColorHelper.FillCellColor(matches, templateSheet);
        var turnoverMarginPolicy = permissionService
            .GetPermissions(FunctionKeys.EventPickingNew)?
            .GetPermissionModel(FunctionKeys.EventPickingNew_EventAndPerformance);
        if (turnoverMarginPolicy is { IsRead: false } || isMiscMatch)
        {
            templateSheet.DeleteColumn(ReportConstant.MarginColumn);
            templateSheet.DeleteColumn(ReportConstant.TurnOverColumn);
        }
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