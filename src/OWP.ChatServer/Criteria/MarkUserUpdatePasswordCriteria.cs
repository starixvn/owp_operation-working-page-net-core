using Fanex.Data.Repository;

namespace OWP.ChatServer.Criteria
{
    public class MarkUserUpdatePasswordCriteria : INonQueryCommand
    {
        public int UserId { get; set; }

        public string GetSettingKey() => "OWP_MarkUserUpdatePassword";


        public bool IsValid() => true;
        
    }
}
