
using OWP.Kpi.LA.Helper;
using OWP.Kpi.LA.Models;
using Xunit;

namespace OWP.LAKpi.UnitTests.LA.Helper
{
    public class CalculateGapHelperUnitTest 
    {
        public class CalculateGapHelperTests
        {
            [Fact]
            public void IsGap_ValidDateRange_ReturnsTrue()
            {
                DateTime now = DateTime.Now;
                DateTime fromDate = new DateTime(now.Year - 1, 1, 1);
                DateTime toDate = new DateTime(now.Year, 12, 31);

                bool result = CalculateGapHelper.IsGap(fromDate, toDate);

                Assert.True(result);
            }

            [Fact]
            public void IsGap_InvalidDateRange_ReturnsFalse()
            {

                DateTime now = DateTime.Now;
                DateTime fromDate = new DateTime(now.Year - 1, 1, 1);
                DateTime toDate = new DateTime(now.Year, 11, 30); // Not December

        
                bool result = CalculateGapHelper.IsGap(fromDate, toDate);


                Assert.False(result);
            }
            [Fact]
            public void CalculateGap_ValidInputs_ReturnsExpectedTotalGameInfo()
            {

                int targetId = 1; 
                TotalGameInfo achievedGame = new TotalGameInfo
                {
                    MultipleMatches = 100,
                    VirtualMatches = 50,
                    MultipleProducts = 200,
                    FiveMajorLeagues = 80,
                    SpecialProducts = 30
                };


                TotalGameInfo result = CalculateGapHelper.CalculateGap(targetId, achievedGame);

                Assert.Equal(0, result.FiveMajorLeagues);
                Assert.Equal(0, result.SpecialProducts);
            }

            [Fact]
            public void CalculateValue_TargetZero_ReturnsZero()
            {
                double achieved = 100;
                double target = 0;

    
                double result = CalculateGapHelper.CalculateValue(achieved, target);

    
                Assert.Equal(0, result);
            }

            [Fact]
            public void CalculateValue_TargetNonZero_ReturnsCorrectDifference()
            {
    
                double achieved = 150;
                double target = 100;

                double result = CalculateGapHelper.CalculateValue(achieved, target);

                Assert.Equal(50, result);
            }
        }

    }
}
