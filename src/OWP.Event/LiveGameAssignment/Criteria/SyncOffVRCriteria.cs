using System;
using Fanex.Data.Repository;

namespace OWP.Event.LiveGameAssignment.Criteria;

public class SyncOffVRCriteria : CriteriaBase
{
    public DateTime EventDate { get; set; }

    public DateTime SyncTime { get; set; }

    public override string GetSettingKey() => "OWP_LiveGame_GetOffVRMatches";

    public override bool IsValid() => true;
}

public class LogActionSyncOffVRCriteria : NonQueryCommand
{
    public DateTime EventDate { get; set; }

    public int ActorId { get; set; }

    public DateTime SyncTime { get; set; }

    public override string GetSettingKey() => "OWP_LiveGame_LogActionSyncOffVR";

    public override bool IsValid() => true;
}

public class GetLastUpdatedSyncOffVRCriteria : CriteriaBase
{
    public DateTime EventDate { get; set; }

    public override string GetSettingKey() => "OWP_LiveGame_GetLastUpdatedSync";

    public override bool IsValid() => true;
}

