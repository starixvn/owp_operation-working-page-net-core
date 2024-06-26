
using Fanex.Data.Repository;

namespace OWP.NewLeagueSetting.Criteria;

public class InitDataCriteria : NonQueryCommand
{
    public string XML { get; set; } = default!;

    public override string GetSettingKey() => "OWP_NewLeague_Init_Data";

    public override bool IsValid() => true;
}