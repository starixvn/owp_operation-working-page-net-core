namespace OWP.NewLeagueSetting.Models;

public record Condition(string Type, string Value);

public class ConditionComparer : IEqualityComparer<Condition>
{
    public bool Equals(Condition x, Condition y)
    {
        if (x == null || y == null)
        {
            return false;
        }

        return string.Equals(x.Type, y.Type, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(x.Value, y.Value, StringComparison.OrdinalIgnoreCase);
    }

    public int GetHashCode(Condition obj)
        => obj.Type.ToLowerInvariant().GetHashCode() ^ obj.Value.ToLowerInvariant().GetHashCode();
}