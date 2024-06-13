namespace OWP.WorkingShift.ViewLog.Models
{
    using System.Collections.Generic;

    public class LogData
    {
        public UserLogModel UserLog { get; set; }

        public Dictionary<int, string> Shifts { get; set; }
    }
}
