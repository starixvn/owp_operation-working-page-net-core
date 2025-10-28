using OWP.EventPicking.Api.Helper;
using OWP.EventPicking.Api.Middleware.Models;
using OWP.EventPicking.Api.Services;

namespace OWP.EventPicking.Api.Handlers;

public class ExportExcelRequest : EventPickingRequest, IRequest<FileResult>;

public class ExportExcelHandler(QueryEventPickingService queryEventPickingService, ExportService exportService)
    : IRequestHandler<ExportExcelRequest, FileResult>
{
    public async Task<FileResult> Handle(ExportExcelRequest request, CancellationToken cancellationToken)
    {
        var matches = await queryEventPickingService.Execute(request, new List<short>());
        return await exportService.BuildFileStream(matches.FilteredMatches, false);
    }
}