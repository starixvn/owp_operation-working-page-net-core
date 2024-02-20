namespace OWP.Event.LiveGameAssignment.Criteria;

using Fanex.Data.Repository;

public class AddMatchSettingCriteria : NonQueryCommand
{
    public int OWPMatchId { get; set; }

    public string DataXml { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey() => "OWP_LiveGameAssignment_AddMatchSetting";

    public override bool IsValid() => OWPMatchId > 0;
}