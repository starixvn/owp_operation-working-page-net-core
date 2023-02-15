namespace OWP.OtherSport.Validation.Annotations;

using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class SportAttribute : ValidationAttribute
{
    private readonly ImmutableList<string> sports = ImmutableList<string>.Empty.AddRange(
        new[]
        {
            "Basketball",
            "Football",
            "Field Hockey",
            "Ice Hockey",
            "Darts",
            "Pool",
            "Table Tennis",
            "Volleyball",
            "Handball",
            "Netball",
            "Cricket",
            "Snooker",
            "Baseball",
            "Badminton",
            "Tennis",
            "Other Sports"
        }
    );

    public override bool IsValid(object value)
    {
        if (string.IsNullOrEmpty(value.ToString()) || !sports.Any(x => x.Equals(value.ToString(), StringComparison.CurrentCultureIgnoreCase)))
        {
            return false;
        }

        return true;
    }
}