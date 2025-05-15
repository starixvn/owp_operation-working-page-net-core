using Fanex.Data.Repository;

namespace OWP.ChatServer.Criteria
{
    public class UpdateChatUserInfoCriteria : INonQueryCommand
    {
        public string UserJson { get; set; }

        public string GetSettingKey() => "OWP_UpdateChatUserId";


        public bool IsValid() => true;
        
    }
}
