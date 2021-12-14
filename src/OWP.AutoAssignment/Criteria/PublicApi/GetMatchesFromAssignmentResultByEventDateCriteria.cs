using Fanex.Data.Repository;
using System;
namespace OWP.AutoAssignment.Criteria.PublicApi
{

    public class GetMatchesFromAssignmentResultByEventDateCriteria: CriteriaBase
    {
        public DateTime EventDate { get; set; }

        public override bool IsValid() => true;

        public override string GetSettingKey() => "OWP_AutoAssignment_GetInfoSiteMatchResultByDate";
    }
}
