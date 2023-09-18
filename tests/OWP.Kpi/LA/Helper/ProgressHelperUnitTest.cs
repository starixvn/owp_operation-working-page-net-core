

using OWP.Kpi.LA.Constant;
using OWP.Kpi.LA.Helper;
using OWP.Kpi.LA.Models;
using Xunit;

namespace OWP.Kpi.UnitTests.LA.Helper;
public class ProgressHelperUnitTest
{
        [Fact]
        public void Evaluate_TotalMatchesAreGreaterThanCondition_ReturnExpect()
        {
            var totalGame = new TotalGameInfo
            {
                VirtualMatches = 1000,
                MultipleMatches = 2335
            };

            var result = ProgressHelper.Evaluate(ConditionConstant.ConditionInfos.ToList(), totalGame);

            Assert.Equal("New", result);
        }

        [Fact]
        public void Evaluate_NotMatchingConditionProgress_ReturnEmpty()
        {
            var totalGame = new TotalGameInfo
            {
                VirtualMatches = 15,
                MultipleMatches = 8
            };

            var result = ProgressHelper.Evaluate(ConditionConstant.ConditionInfos.ToList(), totalGame);

            Assert.Equal("", result);
        }

        [Fact]
        public void Evaluate_MatchingConditionProgress_ReturnExpect()
        {
            var totalGame = new TotalGameInfo
            {
                VirtualMatches = 3300,
                MultipleMatches = 900
            };

            var result = ProgressHelper.Evaluate(ConditionConstant.ConditionInfos.ToList() , totalGame);

            Assert.Equal("Level D", result);
        }
}

