namespace OWP.SimpleDataApi.Criteria.UserManagement
{
    using Fanex.Data.Repository;

    public class SyncUsersCriteria : CriteriaBase, INonQueryCommand
    {
        public override string GetSettingKey() => "OWP_API_SyncUsers_From_AdminSite";

        public override bool IsValid() => true;
    }
}