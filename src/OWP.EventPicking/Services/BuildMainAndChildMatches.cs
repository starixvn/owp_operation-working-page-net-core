using OWP.EventPicking.Models;
using OWP.Extensions;

namespace OWP.EventPicking.Services;

public interface BuildMainAndChildMatches
{
    Task<List<EventPickingModel>> GetMainAndChildMatchEvent(DateTime date);
}

public class BuildMainAndChildMatchesImp : BuildMainAndChildMatches
{
    private readonly Repository repository;
    private readonly BuildMainMatchMaxBet buildMainMatchMaxBet;
    private readonly BuildChildMatchMaxBet buildChildMatchMaxBet;
    private readonly BuildRowColor buildRowColor;

    public BuildMainAndChildMatchesImp(
        Repository repository,
        BuildMainMatchMaxBet buildMainMatchMaxBet,
        BuildChildMatchMaxBet buildChildMatchMaxBet,
        BuildRowColor buildRowColor)
    {
        this.repository = repository;
        this.buildMainMatchMaxBet = buildMainMatchMaxBet;
        this.buildChildMatchMaxBet = buildChildMatchMaxBet;
        this.buildRowColor = buildRowColor;
    }

    public async Task<List<EventPickingModel>> GetMainAndChildMatchEvent(DateTime date)
    {
        var mainMatches = await repository.QueryMainMatch(date);
        var childMatches = await repository.QueryChildMatch(date);

        await buildMainMatchMaxBet.BuildMainMatchMaxbet(mainMatches);
        await buildChildMatchMaxBet.BuildChildMatchMaxbet(mainMatches, childMatches);

        //var filteredMainMatches = RemoveTestingMatches(mainMatches);
        var filteredMainMatches = mainMatches;
        var allMatches = CombineMainAndChildMatchEvent(filteredMainMatches, childMatches);
        buildRowColor.BuildRowColor(allMatches);

        return allMatches;
    }

    private static List<EventPickingModel> CombineMainAndChildMatchEvent(IEnumerable<EventPickingModel> mainMatches, IEnumerable<EventPickingModel> childMatches)
    {
        var matches = new List<EventPickingModel>();

        foreach (var mainMatch in mainMatches)
        {
            matches.Add(mainMatch);
            var childMatchesOfThisMainMatch = childMatches
                .Where(childMatch => childMatch.InfoSiteMatchId == mainMatch.InfoSiteMatchId)
                .OrderByDescending(x => x.MaxBetHDPNumber);
            matches.AddRange(childMatchesOfThisMainMatch);
        }

        return matches
            .OrderBy(x => x.EventDateTime)
            .Where(x => !CheckMatchHaveBeenExclude(x.HomeName))
            .ToList();
    }

    private static IEnumerable<EventPickingModel> RemoveTestingMatches(IEnumerable<EventPickingModel> mainMatches)
        => mainMatches.Where(x => !x.IsTestMatch);

    private static bool CheckMatchHaveBeenExclude(string homeName)
        => homeName.IsContains("(ET)") || homeName.IsContains("(PEN)");
}