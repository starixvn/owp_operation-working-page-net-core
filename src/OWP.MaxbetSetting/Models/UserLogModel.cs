namespace OWP.MaxbetSetting.Models
{
    using System;

    public class UserLogModel
    {
        public int Action { get; set; }

        public string DataOld { get; set; }

        public string Data { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }
    }
}