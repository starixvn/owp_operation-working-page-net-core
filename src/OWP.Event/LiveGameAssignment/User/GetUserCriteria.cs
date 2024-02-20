namespace OWP.Event.LiveGameAssignment.User;

using System;
using Fanex.Data.Repository;

public class GetUserCriteria : CriteriaBase
{
    public DateTime EventDate { get; set; }

    public override string GetSettingKey() => "OWP_EventAssignment_GetUser";

    public override bool IsValid() => true;
}
