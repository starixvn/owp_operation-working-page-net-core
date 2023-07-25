using Fanex.Data.Repository;

namespace OWP.Criteria
{
    public class UpdateAccessCounterCriteria : NonQueryCommand
    {
        public int ActorId { get; set; }

        public string FunctionName { get; set; }

        public override bool IsValid() => ActorId > 0 && FunctionName != null;

        public override string GetSettingKey() => "OWP_AccessCounter_Update";
    }
}