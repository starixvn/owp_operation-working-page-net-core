using MediatR;
using Microsoft.AspNetCore.Mvc;
using OWP.EventPicking.Middleware.Models;
using OWP.EventPicking.Services;
using OWP.Services;

namespace OWP.EventPicking.Handlers;

public class ExportExcelRequest : EventPickingRequest, IRequest<FileResult> { }

public class ExportExcelHandler : IRequestHandler<ExportExcelRequest, FileResult>
{
    private readonly QueryEventPickingService queryEventPickingService;

    private readonly ExportService exportService;

    public ExportExcelHandler(QueryEventPickingService queryEventPickingService, ConfigurationService configurationService, ExportService exportService)
    {
        this.queryEventPickingService = queryEventPickingService;
        this.exportService = exportService;
    }

    public async Task<FileResult> Handle(ExportExcelRequest request, CancellationToken cancellationToken)
    {
        var matches = await queryEventPickingService.Execute(request);
        return await exportService.BuildFileStream(matches);
    }
}