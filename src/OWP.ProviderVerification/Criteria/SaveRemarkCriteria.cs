namespace OWP.ProviderVerification.Criteria;

public class SaveRemarkCriteria : NonQueryCommand
{
    public int OwpMatchId { get; set; }

    public string Remark { get; set; }

    public override string GetSettingKey() => "OWP_ProviderVerification_SaveRemark";

    public override bool IsValid() => true;
}