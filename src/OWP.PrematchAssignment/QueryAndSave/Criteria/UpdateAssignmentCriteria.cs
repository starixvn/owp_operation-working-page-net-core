using Fanex.Data.Repository;

namespace OWP.PrematchAssignment.Criteria;

public class UpdateAssignmentCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public int PreMatchLeagueId { get; set; }

    public string HDP { get; set; }

    public string OU { get; set; }

    public string HDPTakeOver { get; set; }

    public string OUTakeOver { get; set; }

    public string OtherHDP { get; set; }

    public string OtherOU { get; set; }

    public string OtherHDPTakeOver { get; set; }

    public string OtherOUTakeOver { get; set; }

    public string Remark { get; set; }

    public override string GetSettingKey() => "OWP_PreMatchAssignment_Update";

    public override bool IsValid() => PreMatchLeagueId > 0 && ActorId > 0;
}