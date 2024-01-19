namespace OWP.Event.LiveGameAssignment.Download.Misc;

using System;
using System.Collections.Generic;
using System.Linq;
using Models.EventPicking;

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

        foreach (var setting in leagueCondition)
        {
            var isFulfillCondition = 0;

            if (setting.Type == Constant.ContainsCondition)
            {
                isFulfillCondition = matchCondition.IsContains(setting.Value) ? 1 : 0;
            }
            else
            {
                isFulfillCondition = matchCondition.Equals(setting.Value, StringComparison.OrdinalIgnoreCase) ? 1 : 0;
            }

            if (isFulfillCondition == 1)
            {
                return 1;
            }
        }

        return 0;
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