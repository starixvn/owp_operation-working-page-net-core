
using Fanex.Data.Repository;

namespace OWP.NewLeagueSetting.Criteria;

public class UpdateConditionCriteria : NonQueryCommand
{
    public string XML { get; set; } = default!;

    public override string GetSettingKey() => "OWP_NewLeague_Update_Condition";

    public override bool IsValid() => true;
}