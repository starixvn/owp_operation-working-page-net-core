using MediatR;
using Microsoft.AspNetCore.Mvc;
using OWP.EventPicking.Helper;
using OWP.EventPicking.Middleware.Models;
using OWP.EventPicking.Models;
using OWP.EventPicking.Services;
using OWP.Extensions;


namespace OWP.EventPicking.Handlers;

public class ExportLSTFormatRequest : EventPickingRequest, IRequest<FileResult> { }

public class ExportLSTFormatHandler : IRequestHandler<ExportLSTFormatRequest, FileResult>
{
    private readonly Repository repository;
    private readonly ExportService exportService;

    public ExportLSTFormatHandler(Repository repository, ExportService exportService)
    {
       this.repository=repository;
        this.exportService = exportService;
    }

    public async Task<FileResult> Handle(ExportLSTFormatRequest request, CancellationToken cancellationToken)
    {
        var matches = await GetMatches(request.Date);
        return  await exportService.BuildFileLSTStream(matches);
    }

    public async Task<IList<ExportLSTFormat>> GetMatches(DateTime date)
    {
        var matchesRaw = await repository.QueryEventPickingESTFormat(date);
        var orderMatches = RemoveTestingMatches(matchesRaw)
            .Where(x => !CheckMatchHaveBeenExclude(x.HomeName))
            .OrderBy(x => x.IsSaba)
            .ThenBy(x => x.EventDateTime)
            .ThenBy(x=> x.InfoSiteMatchId)
            .ToList();
        var matches = await BuildProviderAndNoInfo(orderMatches);

        return matches;
    }

    public async Task<IList<ExportLSTFormat>> BuildProviderAndNoInfo(List<ExportLSTFormat> matches)
    {
        var priorityProviders = await repository.GetProviderArrangement();

        for (int i = 0; i < matches.Count; i++)
        {
            matches[i].No = i + 1;
            matches[i].Agent = ProviderHelper.BuildProviderByOtherProvider(priorityProviders, matches[i].OtherProvider);
        }

        return matches;
    }
    private static bool CheckMatchHaveBeenExclude(string homeName)
        => homeName.IsContains("(ET)") || homeName.IsContains("(PEN)");

    private static IEnumerable<ExportLSTFormat> RemoveTestingMatches(IEnumerable<ExportLSTFormat> matches)
        => matches.Where(x => !x.IsTestMatch);

}
