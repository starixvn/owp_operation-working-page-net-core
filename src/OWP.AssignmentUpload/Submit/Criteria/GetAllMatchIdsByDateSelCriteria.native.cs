namespace OWP.AssignmentUpload.Submit.Criteria;

using Fanex.Data.Repository;

public class GetAllMatchesByDateSelCriteria : CriteriaBase
{
    public DateTime EventDate { get; set; }

    public override string GetSettingKey() => "OWP_AssignmentUpload_AllMatchIdsByDateSel";

    public override bool IsValid() => true;
}