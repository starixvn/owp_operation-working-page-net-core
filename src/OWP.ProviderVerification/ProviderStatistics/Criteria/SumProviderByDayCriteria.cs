namespace OWP.ProviderVerification.ProviderStatistics.Criteria;
public class SumProviderByDayCriteria: SumProviderCriteria
{
    public override string GetSettingKey()=> "OWP_ProviderStatistic_SumByDay";

    public override bool IsValid() => true;
}
