using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OWP.EventPicking.Services;
using OWP.Models;
using OWP.Services;

namespace OWP.EventPicking.Handlers;

public class ExportMiscMatchRequest : IRequest<FileResult>
{
    public DateTime Date { get; set; }
}

public class ExportMiscMatchHandler : IRequestHandler<ExportMiscMatchRequest, FileResult>
{
    private readonly ExportService exportService;

    private readonly BuildMainAndChildMatches buildMainAndChildMatches;

    private readonly AccessCounter accessCounter;

    private readonly AccessCounterModel counterModel;

    public ExportMiscMatchHandler(
        BuildMainAndChildMatches buildMainAndChildMatches,
        AccessCounter accessCounter,
        IOptions<AccessCounterModel> counterOptions,
        ExportService exportService)
    {
        this.buildMainAndChildMatches = buildMainAndChildMatches;
        this.accessCounter = accessCounter;
        this.counterModel = counterOptions.Value;
        this.exportService = exportService;
    }

    public async Task<FileResult> Handle(ExportMiscMatchRequest request, CancellationToken cancellationToken)
    {
        var matches = await buildMainAndChildMatches.GetMainAndChildMatchEvent(request.Date);

        var fileStreamResult = await exportService.BuildFileStream(matches);
        fileStreamResult.FileDownloadName = $"Event Picking-{request.Date:yyyy-MM-dd}.xlsx";
        await accessCounter.UpdateCounter(counterModel.ExportMiscMatch);
        return fileStreamResult;
    }
}