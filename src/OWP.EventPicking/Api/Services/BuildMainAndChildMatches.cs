using OfficeOpenXml.FormulaParsing.Ranges;
using OWP.EventPicking.Api.Helper;
using OWP.EventPicking.Api.Models;
using OWP.MaxbetSetting.Handlers;
using OWP.MaxbetSetting.Models;

namespace OWP.EventPicking.Api.Services;

public interface BuildMainAndChildMatches
{
    Task<List<EventPickingModel>> GetMainAndChildMatchEvent(DateTime date);

    Task<List<EventPickingModel>> GetMainEvent(DateTime date);

    Task<List<EventPickingModel>> GetChildEventEventPicking(DateTime date);

    List<EventPickingModel> CombineMainAndChildMatchEvent(IEnumerable<EventPickingModel> mainMatches,
        List<EventPickingModel> childMatches);
}

public class BuildMainAndChildMatchesImp : BuildMainAndChildMatches
{
    private readonly Repository repository;
    private readonly BuildChildMatchMaxBet buildChildMatchMaxBet;
    private readonly BuildMatchOfferByLiveSetting buildMatchOffer;
    private readonly BuildRowColor buildRowColor;
    private readonly BuildProvider buildProvider;
    private readonly IMediator mediator;

    public BuildMainAndChildMatchesImp(
        Repository repository,
        BuildChildMatchMaxBet buildChildMatchMaxBet,
        BuildRowColor buildRowColor,
        BuildMatchOfferByLiveSetting buildMatchOffer,
        BuildProvider buildProvider,
        IMediator mediator)
    {
        this.repository = repository;
        this.buildChildMatchMaxBet = buildChildMatchMaxBet;
        this.buildRowColor = buildRowColor;
        this.mediator = mediator;
        this.buildProvider = buildProvider;
        this.buildMatchOffer = buildMatchOffer;
    }

    public async Task<List<EventPickingModel>> GetMainAndChildMatchEvent(DateTime date)
    {
        var mainMatches = await repository.QueryMainMatch(date);
        var childMatches = await repository.QueryChildMatch(date);
        var maxBet = await GetMaxBetDefaultSetting();

        buildChildMatchMaxBet.Build(mainMatches, childMatches, maxBet);
        buildMatchOffer.Build(mainMatches, childMatches, maxBet);
        var filteredMainMatches = RemoveTestingMatches(mainMatches);
        var allMatches = CombineMainAndChildMatchEvent(filteredMainMatches, childMatches);
        buildRowColor.BuildRowColor(allMatches);
        await buildProvider.Build(allMatches);

        return allMatches;
    }

    public async Task<List<EventPickingModel>> GetMainEvent(DateTime date)
    {
        var mainMatches = repository.QueryMainMatch(date);
        var maxBet = GetMaxBetDefaultSetting();
        var rAndDInfo = repository.GetRAndDUpdateInfo(date);
        await Task.WhenAll(mainMatches, maxBet, rAndDInfo);
        buildMatchOffer.BuildForMainMatch(mainMatches.Result, maxBet.Result);
        var filteredMainMatches = RemoveTestingMatches(mainMatches.Result)
            .Where(x => !CheckMatchHaveBeenExclude(x.HomeName))
            .OrderBy(x => x.IsSaba)
            .ThenBy(x => x.EventDateTime)
            .ToList();

        RAndDInfoHelper.Build(filteredMainMatches, rAndDInfo.Result);
        buildRowColor.BuildRowColor(filteredMainMatches);
        await buildProvider.Build(filteredMainMatches);
        return filteredMainMatches;
    }

    public async Task<List<EventPickingModel>> GetChildEventEventPicking(DateTime date)
    {
        var generatedChildMatch = await repository.QueryEventPickingChildMatch(date);
        SpecialProductHelper.BuildSpecialProduct(generatedChildMatch);
        buildRowColor.BuildRowColor(generatedChildMatch);
        await buildProvider.Build(generatedChildMatch);

        return generatedChildMatch;
    }


    public  List<EventPickingModel> CombineMainAndChildMatchEvent(IEnumerable<EventPickingModel> mainMatches, List<EventPickingModel> childMatches)
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
            .Where(x => !CheckMatchHaveBeenExclude(x.HomeName))
            .OrderBy(x => x.IsSaba)
            .ThenBy(x => x.EventDateTime)
            .ToList();
    }

    private async Task<Dictionary<decimal, MaxBetSettingModel>> GetMaxBetDefaultSetting()
    {
        var maxBet = await mediator.Send(new QueryMaxbetSettingRequest());
        return maxBet.ToDictionary(x => x.FT_HDP);
    }

    private static IEnumerable<EventPickingModel> RemoveTestingMatches(IEnumerable<EventPickingModel> mainMatches)
        => mainMatches.Where(x => !x.IsTestMatch);

    private static bool CheckMatchHaveBeenExclude(string homeName)
        => homeName.IsContains("(ET)") || homeName.IsContains("(PEN)");
}