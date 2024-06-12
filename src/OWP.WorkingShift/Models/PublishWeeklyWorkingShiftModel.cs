namespace OWP.WorkingShift.Models
{
    using System;

    public class PublishWeeklyWorkingShiftModel
    {
        
        public int SectionId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public bool IsPublished { get; set; }
    }
}