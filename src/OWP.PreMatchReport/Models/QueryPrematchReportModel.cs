namespace OWP.PreMatchReport.Models
{
    public class QueryPrematchReportModel
    {
        public int UserId {get; set;}
        public string TraderName { get; set; }
        public int GroupId { get; set; }
        public int Bettype { get; set; }
        public int Amount { get; set; }
    }
}