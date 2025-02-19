using OWP.EventPicking.Api.Constant;
using OWP.EventPicking.Api.Helper;
using OWP.EventPicking.Api.Middleware.Models;
using OWP.EventPicking.Api.Services;

namespace OWP.EventPicking.Api.Handlers;

public class ExportExcelRequest : EventPickingRequest, IRequest<FileResult>;

public class ExportExcelHandler : IRequestHandler<ExportExcelRequest, FileResult>
{
    private readonly QueryEventPickingService queryEventPickingService;

    private readonly ExportService exportService;

    public ExportExcelHandler(QueryEventPickingService queryEventPickingService, ExportService exportService)
    {
        this.queryEventPickingService = queryEventPickingService;
        this.exportService = exportService;
    }

    public async Task<FileResult> Handle(ExportExcelRequest request, CancellationToken cancellationToken)
    {
        var matches = await queryEventPickingService.Execute(request);
        var mainMatches = ExcelFilterHelper.ExcludeChildMatch(matches);
        var filterMatchesByIncludeText = ExcelFilterHelper.FilterLeagueName(mainMatches, ReportConstant.HomeTeamAwayTeam);

        return await exportService.BuildFileStream(filterMatchesByIncludeText, false);
    }
}