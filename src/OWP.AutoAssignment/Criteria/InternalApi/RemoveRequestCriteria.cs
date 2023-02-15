namespace OWP.AutoAssignment.Criteria.InternalApi;

using Fanex.Data.Repository;

public class RemoveRequestCriteria : NonQueryCommand
{
    public string RequestId { get; set; }

    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_AutoAssignment_RemoveRequestId";
}