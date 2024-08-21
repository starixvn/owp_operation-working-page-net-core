using OWP.EventPicking.Api.Helper;
using OWP.EventPicking.Api.Middleware.Models;
using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Services;

namespace OWP.EventPicking.Api.Handlers;

public class ExportLSTFormatRequest : EventPickingRequest, IRequest<FileResult>;

public class ExportLSTFormatHandler(Repository repository, ExportService exportService) : IRequestHandler<ExportLSTFormatRequest, FileResult>
{
    public async Task<FileResult> Handle(ExportLSTFormatRequest request, CancellationToken cancellationToken)
    {
        var matches = await GetMatches(request.Date);
        return await exportService.BuildFileLSTStream(matches);
    }

    public async Task<IList<ExportLSTFormat>> GetMatches(DateTime date)
    {
        var mainMatches =  GetMainMatches(date);
        var childMatches = GetChildMatch(date);
        await Task.WhenAll(mainMatches, childMatches);
        var allMatchesRaw =
            SpecialProductHelper.CombineMainAndChildMatchEvent(mainMatches.Result, childMatches.Result.ToList());
        var allMatches = await BuildProviderAndNoInfo(allMatchesRaw);
        return allMatches;
    }

    public async Task<IList<ExportLSTFormat>> GetMainMatches(DateTime date)
    {
        var matchesRaw = await repository.QueryEventPickingESTFormat(date);
        var orderMatches = RemoveTestingMatches(matchesRaw)
            .Where(x => !CheckMatchHaveBeenExclude(x.HomeName))
            .OrderBy(x => x.IsSaba)
            .ThenBy(x => x.EventDateTime)
            .ThenBy(x => x.InfoSiteMatchId)
            .ToList();

        return orderMatches;
    }

    public async Task<IList<ExportLSTFormat>> GetChildMatch(DateTime date)
    {
        var matches = (await repository.QueryChildMatchESTFormat(date)).ToList();
        SpecialProductHelper.BuildSpecialProduct(matches);
        return matches;
    }

    public async Task<IList<ExportLSTFormat>> BuildProviderAndNoInfo(List<ExportLSTFormat> matches)
    {
        var priorityProviders = await repository.GetProviderArrangement();

        for (var i = 0; i < matches.Count; i++)
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
