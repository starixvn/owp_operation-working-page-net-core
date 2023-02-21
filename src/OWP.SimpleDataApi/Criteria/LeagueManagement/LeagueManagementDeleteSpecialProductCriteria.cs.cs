using Fanex.Data.Repository;

namespace OWP.SimpleDataApi.Criteria.LeagueManagement;

public class LeagueManagementDeleteSpecialProductCriteria : NonQueryCommand
{
    public int ProductId { get; set; }

    public int ActorId { get; set; }

    public override string GetSettingKey() => "OWP_SpecialProduct_Delete";

    public override bool IsValid() => ProductId > 0;
}