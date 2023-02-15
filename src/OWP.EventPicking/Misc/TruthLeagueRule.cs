namespace OWP.EventPicking.Misc;

using Extensions;
using Models;

public class TruthLeagueRule
{
    private TruthLeagueRule nextRule;

    public void SetSuccessor(TruthLeagueRule next) => nextRule = next;

    public virtual int Execute(IEnumerable<Condition> leagueCondition, string matchCondition)
    {
        return nextRule != null
            ? nextRule.Execute(leagueCondition, matchCondition)
            : -1;
    }
}

public class MatchContainsAnyLeagueItemRule : TruthLeagueRule
{
    public override int Execute(IEnumerable<Condition> leagueCondition, string matchCondition)
    {
        if (!leagueCondition.Any() || string.IsNullOrEmpty(matchCondition))
        {
            return base.Execute(leagueCondition, matchCondition);
        }

        var isFulfillCondition = leagueCondition.Any(x => x.Type == Constant.ContainsCondition
                ? matchCondition.IsContains(x.Value)
                : matchCondition.Equals(x.Value, StringComparison.OrdinalIgnoreCase));
        if (isFulfillCondition)
        {
            return 1;
        }

        return 0;
    }
}

public class BothAreEmptyRule : TruthLeagueRule
{
    public override int Execute(IEnumerable<Condition> leagueCondition, string matchCondition)
    {
        if (!leagueCondition.Any() && string.IsNullOrEmpty(matchCondition))
        {
            return 1;
        }

        return base.Execute(leagueCondition, matchCondition);
    }
}

public class LeagueHasValueAndMatchIsEmptyRule : TruthLeagueRule
{
    public override int Execute(IEnumerable<Condition> leagueCondition, string matchCondition)
    {
        if (leagueCondition.Any() && string.IsNullOrEmpty(matchCondition))
        {
            return 0;
        }

        return base.Execute(leagueCondition, matchCondition);
    }
}