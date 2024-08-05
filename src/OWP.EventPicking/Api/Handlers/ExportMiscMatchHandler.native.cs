using OWP.Constants;
using OWP.EventPicking.Api.Services;

namespace OWP.EventPicking.Api.Handlers;

public class ExportMiscMatchRequest : IRequest<FileResult>
{
    public DateTime Date { get; set; }
}

public class ExportMiscMatchHandler : IRequestHandler<ExportMiscMatchRequest, FileResult>
{
    private readonly ExportService exportService;

    private readonly BuildMainAndChildMatches buildMainAndChildMatches;

    private readonly AccessCounter accessCounter;

    public ExportMiscMatchHandler(
        BuildMainAndChildMatches buildMainAndChildMatches,
        AccessCounter accessCounter,
        ExportService exportService)
    {
        this.buildMainAndChildMatches = buildMainAndChildMatches;
        this.accessCounter = accessCounter;
        this.exportService = exportService;
    }

    public async Task<FileResult> Handle(ExportMiscMatchRequest request, CancellationToken cancellationToken)
    {
        var matches = await buildMainAndChildMatches.GetMainAndChildMatchEvent(request.Date);

        var fileStreamResult = await exportService.BuildFileStream(matches, true);
        fileStreamResult.FileDownloadName = $"Event Picking-{request.Date:yyyy-MM-dd}.xlsx";
        await accessCounter.UpdateCounter(AccessCounterConst.ExportMiscMatch);
        return fileStreamResult;
    }
}