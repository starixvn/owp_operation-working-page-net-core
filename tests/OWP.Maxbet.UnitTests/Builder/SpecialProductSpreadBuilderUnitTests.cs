using NSubstitute;
using OWP.Api.Maxbet;
using OWP.Api.Maxbet.Builder;
using OWP.Api.Maxbet.Models;
using Xunit;

namespace OWP.Maxbet.UnitTests.Builder;

using Xunit.Asserts.Compare;

public class SpecialProductSpreadBuilderUnitTests
{
    private readonly MaxbetRepository syncMaxbetRepository;

    private readonly SpecialProductSpreadBuilder specialProductSpreadBuilder;

    public SpecialProductSpreadBuilderUnitTests()
    {
        syncMaxbetRepository = Substitute.For<MaxbetRepository>();
        specialProductSpreadBuilder = new SpecialProductSpreadBuilderImp(syncMaxbetRepository);
    }

    [Fact]
    public async Task BuildSpreadXMins_Not_Found_Spread_Return_ModelDefaultMaxbetSpread()
    {
        syncMaxbetRepository
            .GetSpreadXMins(123)
            .ReturnsForAnyArgs(new List<decimal?>());
        var actual = await specialProductSpreadBuilder
            .BuildSpreadXMins(123);
        DeepAssert.Equal(new MaxbetSpread(), actual);
    }

    [Fact]
    public async Task BuildTraderXMins_Not_Trader_Return_ModelTrader()
    {
        syncMaxbetRepository
            .GetSpreadXMins(123)
            .ReturnsForAnyArgs(new List<decimal?> { 123 });
        var actual = await specialProductSpreadBuilder
            .BuildSpreadXMins(123);
        DeepAssert.Equal(new MaxbetSpread
        {
            Handicap = 123,
            OverUnder = 123
        }, actual);
    }
}