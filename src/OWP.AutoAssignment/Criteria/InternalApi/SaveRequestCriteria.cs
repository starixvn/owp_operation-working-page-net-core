using Fanex.Data.Repository;

namespace OWP.AutoAssignment.Criteria.InternalApi;

public class SaveRequestCriteria : NonQueryCommand
{
    public string RequestId { get; set; }

    public int Status { get; set; }

    public string AutoAssignmentResultDoc { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override bool IsValid() => !string.IsNullOrEmpty(RequestId) && !string.IsNullOrEmpty(AutoAssignmentResultDoc);

    public override string GetSettingKey() => "OWP_AutoAssignmentResult_Insert";
}