namespace OWP.LiveDetail.User
{
    public class UserInfoModel : ShortUserInfo
    {
        public string Skype { get; set; }

        public string Extension { get; set; }

        public string Phone { get; set; }

        public bool IsSecondAccount { get; set; }
    }
}