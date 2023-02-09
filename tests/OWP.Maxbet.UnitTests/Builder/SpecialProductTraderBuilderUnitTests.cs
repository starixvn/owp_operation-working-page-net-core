using NSubstitute;
using OWP.Maxbet.Builder;
using OWP.Services;

using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.Maxbet.UnitTests.Builder;

public class SpecialProductTraderBuilderUnitTests
{
    private readonly MaxbetRepository syncMaxbetRepository;

    private readonly SpecialProductTraderBuilder specialProductTraderBuilderImp;

    private readonly LogServiceWrapper logServiceWrapper;

    public SpecialProductTraderBuilderUnitTests()
    {
        logServiceWrapper = Substitute.For<LogServiceWrapper>();
        syncMaxbetRepository = Substitute.For<MaxbetRepository>();
        specialProductTraderBuilderImp = new SpecialProductTraderBuilderImp(syncMaxbetRepository, logServiceWrapper);
    }

    [Fact]
    public async void BuildTraderXMins_Not_Found_Trader_Return_ModelDefaultTrader()
    {
        syncMaxbetRepository
            .GetTraderXMins(123)
            .ReturnsForAnyArgs(new List<int?>());
        var actual = await specialProductTraderBuilderImp
            .BuildTraderXMins(123);
        DeepAssert.Equal(new(), actual);
    }

    [Fact]
    public async void BuildTraderXMins_Not_Trader_Return_ModelTrader()
    {
        syncMaxbetRepository
            .GetTraderXMins(123)
            .ReturnsForAnyArgs(new List<int?>() { 123 });
        var actual = await specialProductTraderBuilderImp
            .BuildTraderXMins(123);
        DeepAssert.Equal(new()
        {
            Handicap = 123,
            OverUnder = 123
        }, actual);
    }
}