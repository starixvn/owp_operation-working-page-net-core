namespace OWP.ProviderVerification.ProviderStatistic.Criteria;
public abstract class SumProviderCriteria: CriteriaBase
{
   public DateTime FromDate { get; set; }

   public DateTime ToDate { get; set; }
}
