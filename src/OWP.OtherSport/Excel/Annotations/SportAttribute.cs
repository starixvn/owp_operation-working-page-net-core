namespace OWP.OtherSport.Excel.Annotations
{
    using System.Collections.Immutable;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class SportAttribute : ValidationAttribute
    {
        public readonly ImmutableList<string> sports = ImmutableList<string>.Empty.AddRange(
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
            if (string.IsNullOrEmpty(value.ToString()) || !sports.Any(x => x.Equals(value.ToString(), System.StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            return true;
        }
    }
}