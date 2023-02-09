using OWP.EventPicking.Misc;
using OWP.EventPicking.Models;
using Xunit;

namespace OWP.EventPicking.UnitTests.Mics;

public class TruthLeagueRuleUnitTests
{
    [Theory]
    [InlineData("TV", 1)]
    [InlineData("test", 0)]
    [InlineData("HasTV", 1)]
    [InlineData("", 0)]
    public void TruthLeagueRule_LeagueConditionHasElements(string matchCondition, int expected)
    {
        var firstRule = new MatchContainsAnyLeagueItemRule();
        var secondRule = new BothAreEmptyRule();
        var thirdRule = new LeagueHasValueAndMatchIsEmptyRule();

        firstRule.SetSuccessor(secondRule);
        secondRule.SetSuccessor(thirdRule);
        var leagueCondition = new List<Condition>
        {
            new() { Type = "Contains", Value = "TV" },
            new() { Type = "Equal", Value = "HasTV" },
        };

        var actual = firstRule.Execute(leagueCondition, matchCondition);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("", 1)]
    public void TruthLeagueRule_LeagueConditionIsEmpty(string matchCondition, int expected)
    {
        var firstRule = new MatchContainsAnyLeagueItemRule();
        var secondRule = new BothAreEmptyRule();
        var thirdRule = new LeagueHasValueAndMatchIsEmptyRule();

        firstRule.SetSuccessor(secondRule);
        secondRule.SetSuccessor(thirdRule);
        var leagueCondition = new List<Condition>();

        var actual = firstRule.Execute(leagueCondition, matchCondition);

        Assert.Equal(expected, actual);
    }
}