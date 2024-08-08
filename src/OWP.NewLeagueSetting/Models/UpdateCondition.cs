namespace OWP.NewLeagueSetting.Models;

[Serializable]
public class UpdateCondition
{
    public string League { get; set; } = default!;

    public string FullConditionDescription { get; set; } = default!;
}
