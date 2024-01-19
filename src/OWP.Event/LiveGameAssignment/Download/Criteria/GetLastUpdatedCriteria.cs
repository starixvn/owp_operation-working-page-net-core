namespace OWP.Event.LiveGameAssignment.Download.Criteria;

using System;
using Fanex.Data.Repository;
public class GetLastUpdatedCriteria : CriteriaBase
{
    public DateTime EventDate { get; set; }

    public override string GetSettingKey() => "OWP_LiveGameAssignment_GetLastUpdated";

    public override bool IsValid() => true;
}
