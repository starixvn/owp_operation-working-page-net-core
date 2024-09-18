using NSubstitute;
using OWP.Api.Maxbet;
using OWP.Api.Maxbet.Builder;
using OWP.Api.Maxbet.Models;
using Xunit;

namespace OWP.Maxbet.UnitTests.Builder;

using Xunit.Asserts.Compare;

public class SpecialProductTraderBuilderUnitTests
{
    private readonly MaxbetRepository syncMaxbetRepository;

    private readonly SpecialProductTraderBuilder specialProductTraderBuilderImp;

    public SpecialProductTraderBuilderUnitTests()
    {
        syncMaxbetRepository = Substitute.For<MaxbetRepository>();
        specialProductTraderBuilderImp = new SpecialProductTraderBuilderImp(syncMaxbetRepository);
    }

    [Fact]
    public async void BuildTraderXMins_Not_Found_Trader_Return_ModelDefaultTrader()
    {
        syncMaxbetRepository
            .GetTraderXMins(123)
            .ReturnsForAnyArgs(new List<int?>());
        var actual = await specialProductTraderBuilderImp
            .BuildTraderXMins(123);
        DeepAssert.Equal(new TraderAssignmentSender(), actual);
    }

    [Fact]
    public async void BuildTraderXMins_Not_Trader_Return_ModelTrader()
    {
        syncMaxbetRepository
            .GetTraderXMins(123)
            .ReturnsForAnyArgs(new List<int?> { 123 });
        var actual = await specialProductTraderBuilderImp
            .BuildTraderXMins(123);
        DeepAssert.Equal(new TraderAssignmentSender
        {
            Handicap = 123,
            OverUnder = 123
        }, actual);
    }
}