namespace OWP.LeagueManagement.Models
{
    using Newtonsoft.Json;
    public class LogDisplayModel
    {
        public string Product { get; set; }
        public string Action { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string ModifyDate { get; set; }
        public string ModifyBy { get; set; }
    }
}