namespace OWP.OvertimeReport.Models
{
    using System;

    public class OvertimeData
    {
        public string UserId { get; set; }

        public string Username { get; set; }

        public DateTime ReportDate { get; set; }

        public string Location { get; set; }

        public decimal Hour { get; set; }

        public bool IsOffDay { get; set; }
    }
}
