namespace OWP.TicketCenter.Criteria;

using Fanex.Data.Repository;

public class BuildNewTicketIdCriteria : CriteriaBase
{
    public int TicketType { get; set; }

    public int SubType { get; set; }
        
    public override string GetSettingKey() => "OWP_TicketIssue_Create";



    public override bool IsValid()
        => true;
}