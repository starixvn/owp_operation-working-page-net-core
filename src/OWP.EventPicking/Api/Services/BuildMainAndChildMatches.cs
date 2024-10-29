using Microsoft.Extensions.Options;
using OWP.EventPicking.Api.Helper;
using OWP.EventPicking.Api.Models;
using OWP.MaxbetSetting.Handlers;
using OWP.MaxbetSetting.Models;
using OWP.Models;

namespace OWP.EventPicking.Api.Services;

public interface BuildMainAndChildMatches
{
    Task<List<EventPickingModel>> GetMainAndChildMatchEvent(DateTime date);

    Task<List<EventPickingModel>> GetMainEvent(DateTime date);

    Task<List<EventPickingModel>> GetChildEventEventPicking(DateTime date);
}

public class BuildMainAndChildMatchesImp(Repository repository,
        BuildChildMatchMaxBet buildChildMatchMaxBet,
        BuildRowColor buildRowColor,
        BuildMatchOfferByLiveSetting buildMatchOffer,
        BuildProvider buildProvider,
        IMediator mediator,
        IOptions<AppSettings> appSettingOptions)
    : BuildMainAndChildMatches
{
    private readonly AppSettings appSettings = appSettingOptions.Value;

    public async Task<List<EventPickingModel>> GetMainAndChildMatchEvent(DateTime date)
    {
        var mainMatches = await repository.QueryMainMatch(date);
        var childMatches = await repository.QueryChildMatch(date);
        var maxBet = await GetMaxBetDefaultSetting();

        buildChildMatchMaxBet.Build(mainMatches, childMatches, maxBet);
        buildMatchOffer.Build(mainMatches, childMatches, maxBet);
        var filteredMainMatches = MainMatchHelper.Filter(mainMatches, appSettings.ShowTestMatchEventPicking);
        var allMatches = SpecialProductHelper.CombineMainAndChildMatchAndAddMatchNo(filteredMainMatches, childMatches);
        buildRowColor.BuildRowColor(allMatches);
        await buildProvider.Build(allMatches);

        return allMatches;
    }

    public async Task<List<EventPickingModel>> GetMainEvent(DateTime date)
    {
        var mainMatches = repository.QueryMainMatch(date);
        var maxBet = GetMaxBetDefaultSetting();
        var rAndDInfo = repository.GetRAndDUpdateInfo(date);
        var (movedEventDateMatches, movedEventDateMatchesRnD) = await repository.QueryMovedEventDateMatch(date);
        await Task.WhenAll(mainMatches, maxBet, rAndDInfo);

        if (movedEventDateMatches.Count > 0)
        {
            mainMatches.Result.AddRange(movedEventDateMatches);
            rAndDInfo.Result.AddRange(movedEventDateMatchesRnD);
        }
        buildMatchOffer.BuildForMainMatch(mainMatches.Result, maxBet.Result);
        var filteredMainMatches = MainMatchHelper.Filter(mainMatches.Result, appSettings.ShowTestMatchEventPicking);

        var filteredMainMatchesSorted = SortEventPicking.SortList(filteredMainMatches);

        RAndDInfoHelper.Build(filteredMainMatchesSorted, rAndDInfo.Result);
        buildRowColor.BuildRowColor(filteredMainMatchesSorted);
        await buildProvider.Build(filteredMainMatchesSorted);
        return filteredMainMatchesSorted;
    }

    public async Task<List<EventPickingModel>> GetChildEventEventPicking(DateTime date)
    {
        var generatedChildMatch = await repository.QueryEventPickingChildMatch(date);
        SpecialProductHelper.BuildSpecialProduct(generatedChildMatch);
        buildRowColor.BuildRowColor(generatedChildMatch, true);
        await buildProvider.Build(generatedChildMatch);

        return generatedChildMatch;
    }

    private async Task<Dictionary<decimal, MaxBetSettingModel>> GetMaxBetDefaultSetting()
    {
        var maxBet = await mediator.Send(new QueryMaxbetSettingRequest());
        return maxBet.ToDictionary(x => x.FT_HDP);
    }
}