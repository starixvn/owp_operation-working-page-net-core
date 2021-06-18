using System.Collections.Generic;

namespace OWP.PreMatchNotification.LinkedMatch.Models
{
    public class InfositePrematchLeague
    {
        public int InfoSiteMatchId { get; set; }

        public int PreMatchLeagueId { get; set; }

        public List<int> PrematchAssigneeIds { get; set; } = new List<int>();
    }
}
