namespace OWP.Report.PreMatchReport.Criteria;

using Fanex.Data.Repository;

public class GetUserIdFromDb02Criteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_AutoAssignment_GetCustInfoFromDB02";

    public override bool IsValid() => true;
}