namespace OWP.WorkingShift.Models
{
    using System;

    public class WorkingShiftQueryByUserModel
    {
        public string UserIds { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
