using NSubstitute;
using OWP.Maxbet.Builder;
using OWP.Maxbet.Models;
using Xunit;

namespace OWP.Maxbet.UnitTests.Builder;

using Xunit.Asserts.Compare;

public class SpecialProductMaxbetBuilderUnitTests
{
    private readonly SpecialProductMaxbetBuilder specialProductMaxbetBuilder;

    private readonly MaxbetRepository syncMaxbetRepository;

    public SpecialProductMaxbetBuilderUnitTests()
    {
        syncMaxbetRepository = Substitute.For<MaxbetRepository>();
        var maxbetBuilder = Substitute.For<MaxbetBuilder>();
        specialProductMaxbetBuilder = new SpecialProductMaxbetBuilderImp(syncMaxbetRepository, maxbetBuilder);
    }

    [Fact]
    public async void BuildMaxbet_Not_Found_Maxbet_Return_ModelDefault()
    {
        syncMaxbetRepository
            .GetMaxbetByOwpMatchId(123)
            .ReturnsForAnyArgs(new List<MaxbetModel>());
        var actual = await specialProductMaxbetBuilder.BuildMaxbet(123);
        DeepAssert.Equal(new MaxbetSpread(), actual);
    }

    [Fact]
    public async void BuildMaxbet_Found_Maxbet_Return_ModelMaxbetSpread()
    {
        syncMaxbetRepository
            .GetMaxbetByOwpMatchId(123)
            .ReturnsForAnyArgs(new List<MaxbetModel>
            {
                new ()
                {
                    BettypeId=1,
                    Value=10
                },
                new ()
                {
                    BettypeId=3,
                    Value=10
                },
                new ()
                {
                    BettypeId=7,
                    Value=10
                },
                new ()
                {
                    BettypeId=8,
                    Value=10
                }
            });
        var actual = await specialProductMaxbetBuilder.BuildMaxbet(123);
        DeepAssert.Equal(new MaxbetSpread
        {
            Handicap = 10,
            OverUnder = 10,
            FirstHalfHandicap = 10,
            FirstHalfOverUnder = 10
        }, actual);
    }

    [Fact]
    public async void BuildMaxbetXMins_Not_Found_Maxbet_Return_ModelDefaultMaxbetSpread()
    {
        syncMaxbetRepository
            .GetMaxbetXMins(123)
            .ReturnsForAnyArgs(new List<decimal?>());
        var actual = await specialProductMaxbetBuilder
            .BuildMaxbetXMins(123);
        DeepAssert.Equal(new MaxbetSpread(), actual);
    }

    [Fact]
    public async void BuildMaxbetXMins_Found_Maxbet_Return_ModelMaxbetSpread()
    {
        syncMaxbetRepository
            .GetMaxbetXMins(123)
            .ReturnsForAnyArgs(new List<decimal?> { 123 });
        var actual = await specialProductMaxbetBuilder
            .BuildMaxbetXMins(123);
        DeepAssert.Equal(new MaxbetSpread
        {
            Handicap = 123,
            OverUnder = 123
        }, actual);
    }
}