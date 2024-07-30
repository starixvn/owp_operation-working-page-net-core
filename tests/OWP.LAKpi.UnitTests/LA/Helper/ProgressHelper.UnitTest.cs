using OWP.Kpi.LA.Helper;
using OWP.Kpi.LA.Models;
using Xunit;

namespace OWP.LAKpi.UnitTests.LA.Helper;

public class ProgressHelperUnitTest
{
    [Fact]
    public void Evaluate_ShouldReturnFail_WhenNoConditionsMatch()
    {
        var conditionInfos = new List<ConditionInfo>
        {
            new()
            {
                Progress = "Fail",
                ConditionFirst = new Condition { VirtualMatches = 10, MultipleMatches = 10 },
                ConditionSecond = new Condition { VirtualMatches = 20, MultipleMatches = 20 }
            }
        };

        var totalGame = new TotalGameInfo { VirtualMatches = 5, MultipleMatches = 5 };

        var result = ProgressHelper.Evaluate(conditionInfos, totalGame);

        Assert.Equal("Fail", result);
    }

    [Fact]
    public void Evaluate_ShouldReturnProgressOfFirstCondition_WhenConditionMatches()
    {
        var conditionInfos = new List<ConditionInfo>
        {
            new()
            {
                Progress = "FirstConditionMet",
                ConditionFirst = new Condition { VirtualMatches = 5, MultipleMatches = 5 },
                ConditionSecond = new Condition { VirtualMatches = 20, MultipleMatches = 20 }
            }
        };

        var totalGame = new TotalGameInfo { VirtualMatches = 10, MultipleMatches = 10 };

        var result = ProgressHelper.Evaluate(conditionInfos, totalGame);

        Assert.Equal("FirstConditionMet", result);
    }

    [Fact]
    public void Evaluate_ShouldReturnProgressOfMatchingCondition_WhenConditionMatches()
    {
        var conditionInfos = new List<ConditionInfo>
        {
            new()
            {
                Progress = "FirstConditionMet",
                ConditionFirst = new Condition { VirtualMatches = 5, MultipleMatches = 5 },
                ConditionSecond = new Condition { VirtualMatches = 20, MultipleMatches = 20 }
            },
            new()
            {
                Progress = "SecondConditionMet",
                ConditionFirst = new Condition { VirtualMatches = 10, MultipleMatches = 10 },
                ConditionSecond = new Condition { VirtualMatches = 15, MultipleMatches = 15 }
            }
        };

        var totalGame = new TotalGameInfo { VirtualMatches = 12, MultipleMatches = 12 };

        var result = ProgressHelper.Evaluate(conditionInfos, totalGame);

        Assert.Equal("SecondConditionMet", result);
    }

    [Fact]
    public void IsGreaterThanOrEqualTo_ShouldReturnTrue_WhenTotalGameIsGreaterThanOrEqualToCondition()
    {
        var totalGame = new TotalGameInfo { VirtualMatches = 10, MultipleMatches = 10 };
        var condition = new TotalGameInfo { VirtualMatches = 5, MultipleMatches = 5 };

        var result = ProgressHelper.IsGreaterThanOrEqualTo(totalGame, condition);

        Assert.True(result);
    }

    [Fact]
    public void IsGreaterThanOrEqualTo_ShouldReturnFalse_WhenTotalGameIsLessThanCondition()
    {
        var totalGame = new TotalGameInfo { VirtualMatches = 5, MultipleMatches = 5 };
        var condition = new TotalGameInfo { VirtualMatches = 10, MultipleMatches = 10 };

        var result = ProgressHelper.IsGreaterThanOrEqualTo(totalGame, condition);

        Assert.False(result);
    }
}