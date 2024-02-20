namespace OWP.Event.LiveGameAssignment.Criteria;

using Fanex.Data.Repository;

public class AddMatchRecommendOddsCriteria : NonQueryCommand
{
    public int OWPMatchId { get; set; }

    public string DataXml { get; set; }

    public int UserId { get; set; }

    public string SessionId { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey() => "OWP_LiveGameAssignment_AddMatchRecommendOdds";

    public override bool IsValid() => OWPMatchId > 0;
}