using Fanex.Data.Repository;

namespace OWP.Criteria;

public class UpdateAccessCounterCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public string FunctionName { get; set; }

    public override bool IsValid()
    {
        return ActorId > 0 && FunctionName != null;
    }

    public override string GetSettingKey()
    {
        return "OWP_AccessCounter_Update";
    }
}