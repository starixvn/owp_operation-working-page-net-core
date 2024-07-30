namespace OWP.DailyWorkingShift.Models
{
    using System;
    using System.Collections.Generic;

    public class QueryUserMatchGroup
    {
        public int OWPMatchId { get; set; }

        public List<int> BetTypes { get; set; }

        public bool isETPK { get; set; }

        public DateTime EventDateTime { get; set; }
    }
}