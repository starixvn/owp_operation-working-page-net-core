namespace OWP.PreMatchAssignment.Criteria
{
    using Fanex.Data.Repository;

    public class QueryLogCriteria : CriteriaBase
    {
        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public bool IsDay { get; set; }

        public override string GetSettingKey() => "OWP_PreMatchAssignment_GetChangeLog";

        public override bool IsValid() => PageNumber > 0 && PageSize > 0;
    }
}