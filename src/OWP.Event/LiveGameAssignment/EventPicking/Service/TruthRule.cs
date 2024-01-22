namespace OWP.Event.LiveGameAssignment.EventPicking.Service;
using OWP.Event.LiveGameAssignment.Download.Misc;
using OWP.Event.LiveGameAssignment.EventPicking.Model;
using System;
using System.Collections.Generic;
using System.Linq;

public class TruthRule
{
    private TruthRule nextRule;

    public void SetSuccessor(TruthRule next) => nextRule = next;

    public virtual int Execute(IEnumerable<Condition> leagueCondition, string matchCondition)
    {
        return nextRule != null
            ? nextRule.Execute(leagueCondition, matchCondition)
            : -1;
    }
}

public class MatchContainsAnyLeagueItemRule : TruthRule
{
    public override int Execute(IEnumerable<Condition> leagueCondition, string matchCondition)
    {
        if (!leagueCondition.Any() || string.IsNullOrEmpty(matchCondition))
        {
            return base.Execute(leagueCondition, matchCondition);
        }

        var isFulfillCondition = leagueCondition.ToList().Exists(x => x.Type == Constant.ContainsCondition
                ? matchCondition.IsContains(x.Value)
                : matchCondition.Equals(x.Value, StringComparison.OrdinalIgnoreCase));

        return isFulfillCondition ? 1 : 0;
    }
}

public class BothAreEmptyRule : TruthRule
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

public class LeagueHasValueAndMatchIsEmptyRule : TruthRule
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