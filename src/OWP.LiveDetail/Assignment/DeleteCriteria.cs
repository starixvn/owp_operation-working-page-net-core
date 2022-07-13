namespace OWP.LiveDetail.Assignment
{
    public class DeleteCriteria : CommandCriteria
    {
        public int AssignmentId { get; set; }

        public override bool IsValid() => base.IsValid() && AssignmentId > 0;

        public override string GetSettingKey() => "OWP_Assignment_Delete";
    }
}