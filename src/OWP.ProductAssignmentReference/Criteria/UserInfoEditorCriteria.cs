using Fanex.Data.Repository;

namespace OWP.ProductAssignmentReference.Criteria
{
    public class UserInfoEditorCriteria : NonQueryCommand
    {
        public int ActorId { get; set; }

        public string Username { get; set; }

        public int UserId { get; set; }

        public string Label { get; set; }

        public string Value { get; set; }

        public override string GetSettingKey()
            => "OWP_ProductAssignment_UserInfo_Update";

        public override bool IsValid() => UserId > 0;
    }
}
