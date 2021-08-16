using NSubstitute;
using OWP.Maxbet.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.Maxbet.UnitTests.Builder
{
    public class SpecialProductTraderBuilderUnitTests
    {
        private readonly MaxbetReposiory syncMaxbetReposiory;

        private readonly SpecialProductTraderBuilder specialProductTraderBuilderImp;

        public SpecialProductTraderBuilderUnitTests()
        {
            syncMaxbetReposiory = Substitute.For<MaxbetReposiory>();
            specialProductTraderBuilderImp = new SpecialProductTraderBuilderImp(syncMaxbetReposiory);
        }

        [Fact]
        public async void BuildTraderXMins_Not_Found_Trader_Return_ModelDefaultTrader()
        {
            syncMaxbetReposiory
                .GetTraderXMins(123)
                    .ReturnsForAnyArgs(new List<int?>());
            var actual = await specialProductTraderBuilderImp
                .BuildTraderXMins(123);
            DeepAssert.Equal(new(), actual);
        }

        [Fact]
        public async void BuildTraderXMins_Not_Trader_Return_ModelTrader()
        {
            syncMaxbetReposiory
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
}
