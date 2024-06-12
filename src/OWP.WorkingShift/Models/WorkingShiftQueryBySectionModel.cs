namespace OWP.WorkingShift.Models
{
    using System;

    public class WorkingShiftQueryBySectionModel
    {
        public string SectionId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}
