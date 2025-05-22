namespace OWP.ProviderVerification.ProviderStatistics.Criteria;
public class SumProviderByLeagueCriteria: SumProviderCriteria
{
    public override string GetSettingKey()=> "OWP_ProviderStatistic_SumByLeague";

    public override bool IsValid() => true;
}
