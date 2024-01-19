namespace Fanex.NAP.OWP.TaskScheduling.Models.TaskListToday
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class AssignmentModel
    {
        public AssignmentModel()
        {
            Handicap = new List<Assignment>();
            HandicapOthers = new List<Assignment>();
            Handicap1st = new List<Assignment>();
            Handicap1stOthers = new List<Assignment>();
            OverUnder = new List<Assignment>();
            OverUnderOthers = new List<Assignment>();
            OverUnder1st = new List<Assignment>();
            OverUnder1stOthers = new List<Assignment>();
            OneXTwo = new List<Assignment>();
            OneXTwoOthers = new List<Assignment>();
            OneXTwo1st = new List<Assignment>();
            OneXTwo1stOthers = new List<Assignment>();
            FastMarket = new List<Assignment>();
            FastMarketOthers = new List<Assignment>();
            AllInOne = new List<Assignment>();
            AllInOneOthers = new List<Assignment>();
            Specific15Mins = new List<Assignment>();
            Specific15MinsOthers = new List<Assignment>();
            Specific10Mins = new List<Assignment>();
            Specific10MinsOthers = new List<Assignment>();
            LA = new List<Assignment>();
            LAOthers = new List<Assignment>();
        }

        public IEnumerable<Assignment> Handicap { get; set; }

        public IEnumerable<Assignment> HandicapOthers { get; set; }

        public IEnumerable<Assignment> Handicap1st { get; set; }

        public IEnumerable<Assignment> Handicap1stOthers { get; set; }

        public IEnumerable<Assignment> OverUnder { get; set; }

        public IEnumerable<Assignment> OverUnderOthers { get; set; }

        public IEnumerable<Assignment> OverUnder1st { get; set; }

        public IEnumerable<Assignment> OverUnder1stOthers { get; set; }

        public IEnumerable<Assignment> OneXTwo { get; set; }

        public IEnumerable<Assignment> OneXTwoOthers { get; set; }

        public IEnumerable<Assignment> OneXTwo1st { get; set; }

        public IEnumerable<Assignment> OneXTwo1stOthers { get; set; }

        public IEnumerable<Assignment> Specific15Mins { get; set; }

        public IEnumerable<Assignment> Specific15MinsOthers { get; set; }

        public IEnumerable<Assignment> Specific10Mins { get; set; }

        public IEnumerable<Assignment> Specific10MinsOthers { get; set; }

        public IEnumerable<Assignment> FastMarket { get; set; }

        public IEnumerable<Assignment> FastMarketOthers { get; set; }

        public IEnumerable<Assignment> AllInOne { get; set; }

        public IEnumerable<Assignment> AllInOneOthers { get; set; }

        public IEnumerable<Assignment> LA { get; set; }

        public IEnumerable<Assignment> LAOthers { get; set; }
    }
}