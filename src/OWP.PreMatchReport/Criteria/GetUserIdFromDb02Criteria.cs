using Fanex.Data.Repository;

namespace OWP.PreMatchReport.Criteria
{
    public class GetUserIdFromDb02Criteria : CriteriaBase
    {
        public override string GetSettingKey() => "OWP_AutoAssignment_GetCustInfoFromDB02";

        public override bool IsValid() => true;
    }
}