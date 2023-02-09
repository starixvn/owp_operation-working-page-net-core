namespace OWP.LeagueManagement.Criteria;

using Fanex.Data.Repository;

public class UpdateTvListCriteria : CriteriaBase
{
    public int ActorId { get; set; }

    public string UpdateTvListXml { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey() => "OWP_LeagueManagement_UpdateTVCondition";

    public override bool IsValid() => ActorId > 0;
}