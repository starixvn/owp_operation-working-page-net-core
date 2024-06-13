namespace OWP.WorkingShift.ViewLog.Models
{
    using System;

    public class UserLogModel
    {
        public DateTime ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }

        public int ActionId { get; set; }

        public string DataOld { get; set; }

        public string Data { get; set; }
    }
}
