
namespace OWP.EventPicking.Misc;

public class TruthLeagueRule
{
    private TruthLeagueRule nextRule;

    public void SetSuccessor(TruthLeagueRule next) => nextRule = next;

    public virtual int Execute(List<Condition> leagueCondition, string matchCondition)
    {
        return nextRule?.Execute(leagueCondition, matchCondition) ?? -1;
    }
}

public class MatchContainsAnyLeagueItemRule : TruthLeagueRule
{
    public override int Execute(List<Condition> leagueCondition, string matchCondition)
    {
        if (!leagueCondition.Any() || string.IsNullOrEmpty(matchCondition))
        {
            return base.Execute(leagueCondition, matchCondition);
        }

        var isFulfillCondition = leagueCondition.Exists(x => x.Type == Constant.ContainsCondition
                ? matchCondition.IsContains(x.Value)
                : matchCondition.Equals(x.Value, StringComparison.OrdinalIgnoreCase));

        return isFulfillCondition ? 1 : 0;
    }
}

public class BothAreEmptyRule : TruthLeagueRule
{
    public override int Execute(List<Condition> leagueCondition, string matchCondition)
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
    public override int Execute(List<Condition> leagueCondition, string matchCondition)
    {
        if (leagueCondition.Any() && string.IsNullOrEmpty(matchCondition))
        {
            return 0;
        }

        return base.Execute(leagueCondition, matchCondition);
    }
}