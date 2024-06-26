using Fanex.Data.Repository;

namespace OWP.SimpleDataApi.Criteria.UserManagement;

public class GetUserSyncModelsCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_API_SyncUsers_Get_Change_From_AdminSite";

    public override bool IsValid() => true;
}