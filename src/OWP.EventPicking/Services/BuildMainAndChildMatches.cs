using MediatR;
using OWP.EventPicking.Helper;
using OWP.EventPicking.Models;
using OWP.Extensions;
using OWP.MaxbetSetting.Handlers;
using OWP.MaxbetSetting.Models;

namespace OWP.EventPicking.Services;

public interface BuildMainAndChildMatches
{
    Task<List<EventPickingModel>> GetMainAndChildMatchEvent(DateTime date);

    Task<List<EventPickingModel>> GetMainEvent(DateTime date);
}

public class BuildMainAndChildMatchesImp : BuildMainAndChildMatches
{
    private readonly Repository repository;
    private readonly BuildMainMatchMaxBet buildMainMatchMaxBet;
    private readonly BuildChildMatchMaxBet buildChildMatchMaxBet;
    private readonly BuildMatchOfferByLiveSetting buildMatchOffer;
    private readonly BuildRowColor buildRowColor;
    private readonly BuildProvider buildProvider;
    private readonly IMediator mediator;

    public BuildMainAndChildMatchesImp(
        Repository repository,
        BuildMainMatchMaxBet buildMainMatchMaxBet,
        BuildChildMatchMaxBet buildChildMatchMaxBet,
        BuildRowColor buildRowColor,
        BuildMatchOfferByLiveSetting buildMatchOffer,
        BuildProvider buildProvider,
        IMediator mediator)
    {
        this.repository = repository;
        this.buildMainMatchMaxBet = buildMainMatchMaxBet;
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

        await buildMainMatchMaxBet.BuildMainMatchMaxbet(mainMatches);
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

        await buildMainMatchMaxBet.BuildMainMatchMaxbet(mainMatches.Result);
        buildMatchOffer.BuildForMainMatch(mainMatches.Result, maxBet.Result);
        RAndDInfoHelper.Build(mainMatches.Result, rAndDInfo.Result);

        var filteredMainMatches = mainMatches.Result
            .OrderBy(x => x.EventDateTime)
            .Where(x => !CheckMatchHaveBeenExclude(x.HomeName))
            .ToList();

        buildRowColor.BuildRowColor(filteredMainMatches);
        await buildProvider.Build(filteredMainMatches);
        LengthPropertyBuilder.Build(filteredMainMatches);
        return filteredMainMatches;
    }

    private static List<EventPickingModel> CombineMainAndChildMatchEvent(IEnumerable<EventPickingModel> mainMatches, List<EventPickingModel> childMatches)
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