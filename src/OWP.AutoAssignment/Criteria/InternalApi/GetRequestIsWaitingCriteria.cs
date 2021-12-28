namespace OWP.AutoAssignment.Criteria.InternalApi
{
    using Fanex.Data.Repository;
    using System;

    public class GetRequestIsWaitingCriteria : CriteriaBase
    {
        public DateTime EventDate { get; set; }

        public override bool IsValid() => true;

        public override string GetSettingKey() => "OWP_AutoAssignment_GetRequestIsWaiting";
    }
}