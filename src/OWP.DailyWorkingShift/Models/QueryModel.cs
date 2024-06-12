namespace OWP.DailyWorkingShift.Models
{
    using System;

    public class QueryModel
    {
        public DateTime? Date { get; set; }

        public string User { get; set; }

        public string SectionId { get; set; }

        public string DepartmentId { get; set; }

        public string ShiftId { get; set; }

        public string LocationCode { get; set; }

        public bool ExcludeClosedAccount { get; set; }

        public DailyWorkingShiftPermission Permission { get; set; }
    }
}