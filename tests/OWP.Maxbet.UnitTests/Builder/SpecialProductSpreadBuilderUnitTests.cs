using NSubstitute;
using OWP.Maxbet.Builder;
using OWP.Services;

using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.Maxbet.UnitTests.Builder
{
  public class SpecialProductSpreadBuilderUnitTests
  {
    private readonly MaxbetRepository syncMaxbetRepository;

    private readonly SpecialProductSpreadBuilder specialProductSpreadBuilder;

    private readonly LogServiceWrapper logServiceWrapper;

    public SpecialProductSpreadBuilderUnitTests()
    {
      logServiceWrapper = Substitute.For<LogServiceWrapper>();
      syncMaxbetRepository = Substitute.For<MaxbetRepository>();
      specialProductSpreadBuilder = new SpecialProductSpreadBuilderImp(syncMaxbetRepository, logServiceWrapper);
    }

    [Fact]
    public async void BuildSpreadXMins_Not_Found_Spread_Return_ModelDefaultMaxbetSpread()
    {
      syncMaxbetRepository
          .GetSpreadXMins(123)
              .ReturnsForAnyArgs(new List<decimal?>());
      var actual = await specialProductSpreadBuilder
          .BuildSpreadXMins(123);
      DeepAssert.Equal(new(), actual);
    }

    [Fact]
    public async void BuildTraderXMins_Not_Trader_Return_ModelTrader()
    {
      syncMaxbetRepository
          .GetSpreadXMins(123)
              .ReturnsForAnyArgs(new List<decimal?>() { 123 });
      var actual = await specialProductSpreadBuilder
          .BuildSpreadXMins(123);
      DeepAssert.Equal(new()
      {
        Handicap = 123,
        OverUnder = 123
      }, actual);
    }
  }
}