using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OWP.ApiSyncWorkingShift.Const;

namespace OWP.ApiSyncWorkingShift.Models
{
    public class WorkingShiftHRPPModel
    {
        public string EmployeeID { get; set; }

        public DateTime? ScheduleDate { get; set; }
        
        public DateTime? ShiftStartTime { get; set; }

        public DateTime? ShiftEndTime { get; set; }

        public DateTime? LastUpdated { get; set; }

        public bool IsDayOff { get; set; }
    }

}
