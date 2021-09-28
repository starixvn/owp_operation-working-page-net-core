using System;

namespace OWP.TicketCenter.Models
{
    public class EventOptionInfoModel
    {
        public int OwpLeagueId { get; set; }

        public string LeagueName { get; set; }

        public int? HomeId { get; set; }

        public int? AwayId { get; set; }

        public string HomeName { get; set; }

        public string AwayName { get; set; }

        public DateTime? EventDateTimeGmt8 { get; set; }

        public bool IsLeague { get; set; }

        public int? UserId { get; set; }

        public string Username { get; set; }
    }
}
