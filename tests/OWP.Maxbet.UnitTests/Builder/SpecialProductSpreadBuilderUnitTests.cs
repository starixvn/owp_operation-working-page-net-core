using NSubstitute;
using OWP.Maxbet.Builder;
using OWP.Services;


using System.Text;

using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.Maxbet.UnitTests.Builder
{
    public class SpecialProductSpreadBuilderUnitTests
    {
        private readonly MaxbetReposiory syncMaxbetReposiory;

        private readonly SpecialProductSpreadBuilder specialProductSpreadBuilder;
        
        private readonly LogServiceWrapper logServiceWrapper;


        public SpecialProductSpreadBuilderUnitTests()
        {
            logServiceWrapper = Substitute.For<LogServiceWrapper>();
            syncMaxbetReposiory = Substitute.For<MaxbetReposiory>();
            specialProductSpreadBuilder = new SpecialProductSpreadBuilderImp(syncMaxbetReposiory, logServiceWrapper);
        }

        [Fact]
        public async void BuildSpreadXMins_Not_Found_Spread_Return_ModelDefaultMaxbetSpread()
        {
            syncMaxbetReposiory
                .GetSpreadXMins(123)
                    .ReturnsForAnyArgs(new List<decimal?>());
            var actual = await specialProductSpreadBuilder
                .BuildSpreadXMins(123);
            DeepAssert.Equal(new(), actual);
        }

        [Fact]
        public async void BuildTraderXMins_Not_Trader_Return_ModelTrader()
        {
            syncMaxbetReposiory
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
